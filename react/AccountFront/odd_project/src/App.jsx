import { useState, useEffect } from 'react'
import './App.css'

function App() {
  const [dailyTotal, setDailyTotal] = useState([]);
  const [allRecord, setAllRecord] = useState([]);
  const [leftAmount, setLeftAmount] = useState(0);
  const [errorLog, setErrorLog] = useState("");
  
  async function Test() {
    setErrorLog("");
    try{
      const res = await fetch("https://localhost:7244/api/WiseTest/search/GetTotals", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({})
      });
      if(!res.ok) throw new Error(`/httpError/notOk ${res.status}`);
      const data = await res.json();
      setDailySum(data);
      let amount = 0;
      data.forEach((r) => {
        if(r.subAmount != null){
          let tempint = parseInt(r.subAmount);
          amount += tempint;
        }
      });
      setLeftAmount(amount);
    }
    catch(e){
      setErrorLog("Test" + e.message);
    }
  }
  function MkRecord(r){
    let catename = "";
    let comment = "";
    let amount = 0;
    let username = "";
    let content = (<div style={{
          display: "flex",
          flexDirection: "row",
          justifyContent: "space-between",
          alignItems: "center"
        }}>
          <div>
            <h3>{catename}</h3>
            <p>{comment}</p>
          </div>
          <div>
            <p>{amount}</p>
            <p>{username}</p>
          </div>
        </div>);
    try{
      catename = r.category;
      comment = r.comment;
      amount = parseInt(r.amount);
      username = r.user_name;
    }
    catch (e){
      setErrorLog(e.message);
    }
    return (
      <>
        {content}
      </>
    );
  }
  function MkLsRecord(){
    const count = 0;
    const contentList = [];
    try{
      allRecord.forEach((r) => {
        contentList.append(MkRecord(r));
      });
    }
    catch(e){
      setErrorLog(e.message);
    }
    return (
      <div>
        {contentList}
      </div>
    );
  }
  /*{
        "id": 3,
        "date": "2025-09-17T00:00:00",
        "category_id": 1,
        "category": "",
        "subCategory_id": 1,
        "subCategory": "",
        "user_id": 0,
        "user_name": "",
        "amount": 0,
        "subCount": 1,
        "subAmount": 100,
        "comment": ""
    } */
  return (
    <>
      <div style={
        {
          display: 'flex',
          flexDirection: 'column',
          justifyContent: 'center',
          alignItems: 'center',
          height: '500px',
          width: '300px'
        }
      }>
        <div style={{
          border: '1px solid rgb(255, 255, 255)',
          height: '100px',
          width: '200px',
          padding: '10px'
        }}>
          <p style={{
            textAlign: 'left'
          }}>餘額: {leftAmount}NTD</p>
        </div>
        <div style={{
          padding: '10px'
        }}>
          <button onClick={Test}>Click me</button>
        </div>
        <div style={{
          margin: '20px'
        }}>
        </div>
      </div>
      
      <div style={{
        height: '100px',
        width: '200px'
      }}>
        <p>這裡是錯誤顯示區</p>
        <div style={{
          height: '80%',
          width: '100%'
        }}>
          <p>{errorLog}</p>
        </div>
      </div>
    </>
  )
}

export default App
