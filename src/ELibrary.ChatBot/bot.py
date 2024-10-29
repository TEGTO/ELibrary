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
        self.db = SQLDatabase.from_uri(chat_config.db_uri)
        self.llm = ChatOpenAI(model=chat_config.model)
        self.toolkit = SQLDatabaseToolkit(db=self.db, llm=self.llm)
        self.tools = self.toolkit.get_tools()
        self.system_message = SystemMessage(content=self._create_system_message())

        self.last_refresh_time = datetime.min
        self._refresh_vector_db_if_needed()
        self._add_retriever_tool()

        self.agent = create_react_agent(self.llm, self.tools, messages_modifier=self.system_message)


    def _create_system_message(self):
        table_names = self.db.get_usable_table_names()
        ground_promt = self.chat_config.ground_promt

        table_names_str = ", ".join(table_names)
        return ground_promt.replace("{table_names}", table_names_str)

    def _query_as_list(self, query):
        res = self.db.run(query)
        if res:
            res = [el for sub in ast.literal_eval(res) for el in sub if el]
            res = [re.sub(r"\b\d+\b", "", string).strip() for string in res]
        return list(set(res))

    def _add_retriever_tool(self):
        description = self.chat_config.retriever_description

        retriever_tool = create_retriever_tool(
            self.retriever,
            name="search_proper_nouns",
            description=description,
        )
        self.tools.append(retriever_tool)

    def _refresh_vector_db_if_needed(self):
        elapsed_time = datetime.now() - self.last_refresh_time
        if elapsed_time >= timedelta(minutes=self.chat_config.refresh_interval_minutes):
            print("Refreshing vector database...")
            self.books = self._query_as_list('SELECT name FROM books')
            self.authors = self._query_as_list('SELECT name FROM authors')
            self.publishers = self._query_as_list('SELECT name FROM publishers')

            self.vector_db = FAISS.from_texts(self.authors + self.books + self.publishers, OpenAIEmbeddings())
            self.retriever = self.vector_db.as_retriever(search_kwargs={"k": 5})

            self.last_refresh_time = datetime.now()

    def query(self, question):
        self._refresh_vector_db_if_needed()

        answer = ""
        for response in self.agent.stream({"messages": [HumanMessage(content=question)]}):
            print(answer)
            answer = response
        return answer
