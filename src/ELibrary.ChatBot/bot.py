import ast
import re
from datetime import datetime, timedelta
from langchain_community.agent_toolkits import SQLDatabaseToolkit
from langchain_core.messages import SystemMessage, HumanMessage
from langchain_core.tools import create_retriever_tool
from langgraph.prebuilt import create_react_agent
from langchain_openai import ChatOpenAI, OpenAIEmbeddings
from langchain_community.vectorstores import FAISS
from langchain_community.utilities import SQLDatabase
from typing import NamedTuple

class ChatConfig(NamedTuple):
    db_uri: str
    api_key: str
    ground_promt: str
    model: str
    retriever_description: str
    refresh_interval_minutes: int

class LangChainAgent:
    def __init__(self, chat_config: ChatConfig):
        self.chat_config = chat_config

        try:
            self.db = SQLDatabase.from_uri(self.chat_config.db_uri)
            print("Database connection successful.")
        except Exception as e:
            print(f"Database connection failed: {e}")
            raise

        self.llm = ChatOpenAI(model=chat_config.model)
        self.toolkit = SQLDatabaseToolkit(db=self.db, llm=self.llm)
        self.tools = self.toolkit.get_tools()

        self.system_message = SystemMessage(content=self._create_system_message())

        self.last_refresh_time = datetime.min

        self._refresh_vector_db()

        self._create_agent()

    def _create_agent(self):
        """
        Create the agent with the latest tools and system message.
        """
        self.agent = create_react_agent(self.llm, self.tools, messages_modifier=self.system_message)

    def _create_system_message(self):
        """
        Create the system message by embedding table names into the prompt.
        """
        table_names = self.db.get_usable_table_names()
        ground_promt = self.chat_config.ground_promt
        table_names_str = ", ".join(table_names)
        return ground_promt.replace("{table_names}", table_names_str)

    def _refresh_vector_db(self):
        """
        Refresh the FAISS vector database and retriever tool based on the latest database data.
        """
        print("Refreshing vector database...")
        self.books = self._query_as_list('SELECT name FROM books')
        self.authors = self._query_as_list('SELECT name FROM authors')
        self.publishers = self._query_as_list('SELECT name FROM publishers')

        if not (self.authors or self.books or self.publishers):
            print("Error: No data available to build FAISS vector database.")
            return

        try:
            combined_data = self.authors + self.books + self.publishers
            self.vector_db = FAISS.from_texts(combined_data, OpenAIEmbeddings())
            self.retriever = self.vector_db.as_retriever(search_kwargs={"k": 5})
            self.last_refresh_time = datetime.now()
            self._update_retriever_tool()
        except Exception as e:
            print(f"Error creating FAISS vector database: {e}")

    def _query_as_list(self, query):
        """
        Run a SQL query and return the results as a processed list.
        """
        try:
            print(f"Running query: {query}")
            res = self.db.run(query)
            print(f"Query result: {res}")
            if res:
                res = [el for sub in ast.literal_eval(res) for el in sub if el]
                res = [re.sub(r"\b\d+\b", "", string).strip() for string in res]
            return list(set(res))
        except Exception as e:
            print(f"Error processing query '{query}': {e}")
            return []

    def _update_retriever_tool(self):
        """
        Update the retriever tool dynamically without recreating the entire agent.
        """
        description = self.chat_config.retriever_description
        retriever_tool = create_retriever_tool(
            self.retriever,
            name="search_proper_nouns",
            description=description,
        )
        self.tools = [tool for tool in self.tools if tool.name != "search_proper_nouns"]
        self.tools.append(retriever_tool)

    def query(self, question):
        """
        Handle a query by refreshing components if needed and delegating to the agent.
        """
        elapsed_time = datetime.now() - self.last_refresh_time
        if elapsed_time >= timedelta(minutes=self.chat_config.refresh_interval_minutes):
            self._refresh_vector_db()
            self._create_agent()

        if not hasattr(self, 'agent'):
            self._create_agent()

        answer = ""
        for response in self.agent.stream({"messages": [HumanMessage(content=question)]}):
            print(answer)
            answer = response
        return answer