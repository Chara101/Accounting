import { useState, useEffect } from "react";
import {Link, useLocation} from "react-router-dom";

export default function Record(){
    const [record, setRecord] = useState(useLocation().state || {});

    return (
        <>
            <Link to={"/RecordList"}></Link>
            <div style={{
                display: "flex",
                flexDirection: "row",
                alignItems: "center",
                justifyContent: "space-between"
            }}>
                <input className="amount" defaultValue={record.amount}></input>
                <button onClick={() => {
                    let temp = record;
                    temp.amount = document.getElementsByClassName("amount").value;
                    setRecord(temp);
                }}>按我儲存</button>
            </div>
        </>
    );
}