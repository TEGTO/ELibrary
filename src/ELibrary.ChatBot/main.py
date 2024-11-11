import os
from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from bot import LangChainAgent, ChatConfig

app = FastAPI()

class ChatRequest(BaseModel):
    query: str

class ChatResponse(BaseModel):
    message: str   

chat_config = ChatConfig(
    os.environ["DB_CONNECTION_STRING"],
    os.environ["OPENAI_API_KEY"], 
    os.environ["GROUND_PROMT"], 
    os.environ["MODEL"], 
    os.environ["RETRIEVER_DESCRIPTION"],
    int(os.environ["REFRESH_DATA_IN_MINUTES"]),
    )
       
chatAgent = LangChainAgent(chat_config)

@app.post("/chat")
async def stream_chat(req: ChatRequest):
    try:
        answer = chatAgent.query(req.query)
        return ChatResponse(message=answer["agent"]["messages"][0].content)
    
    except Exception as e:
        print(f"Error processing request: {e}")
        raise HTTPException(status_code=500, detail="Internal Server Error")