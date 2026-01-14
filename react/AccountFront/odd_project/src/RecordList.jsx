import {useState, useEffect, useRef} from "react";
import {Link} from "react-router-dom";

export default function RecordList(){
    const recordList = useRef([]);
    const [totalAmount, setTotalAmount] = useState(0);
    const [totalCount, setTotalCount] = useState(0);
    const [showRecordList, setShowRecordList] = useState([]);
    useEffect(() => {
        try{
        (async () => {
            const res = await fetch("https://localhost:7244/api/WiseTest/search/GetAllTotals", {
                method: "Get"
            })
            if(!res.ok) throw new Error("fetchTotals failed.");
            const data = await res.json();
            let total = 0;
            let count = 0;
            data.forEach((element) => {
                count += element.subCount;
                total += element.subAmount;
            });
            setTotalCount(count);
            setTotalAmount(total);
        }
        )}
        catch(e){
            console.log(e.message);
        }
    }, []);

    useEffect(() => {
        (async () => {
        try{
            const temp = [];
            const res = await fetch("https://localhost:7244/api/WiseTest/search/GetAllRecord", {
                method: "Get"
            });
            if(!res.ok) throw new Error("/Initialize/fail-to-fetch");
            const data = await res.json();
            recordList.current = data;
            data.forEach((element) => {
                temp.push((
                        <Link key={element.id} to={`/Record/${element.id}`} state={element} style={{
                            display: "block",
                            width: "100%"
                        }}>
                            <div style={{
                                display: "flex",
                                flexDirection: "row",
                                alignItems: "center",
                                justifyContent: "space-between",
                                border: "1px solid rgb(255, 255, 255)",
                                width: "100%"
                            }}>
                                <div style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "flex-start",
                                    justifyContent: "space-between"
                                }}>
                                    <p>{String(element.category) ?? "category"}</p>
                                    <p>{String(element.subCategory) ?? "comment"}</p>
                                </div>
                                <div style={{
                                    display: "flex",
                                    flexDirection: "column",
                                    alignItems: "flex-start",
                                    justifyContent: "space-between"
                                }}>
                                    <p>{String(element.amount) ?? "amount"}</p>
                                    <p>{String(element.user_name) ?? "username"}</p>
                                </div>
                            </div>
                        </Link>
                        )
                    );
            });
            setShowRecordList(temp);
        }
        catch(e){
            console.log("/RecordList/" + e.message);
        }}
    )(); //執行async外層lambda
    }, []);

    return (
    <>
        <div>
            <Link to={"/"}>按我返回</Link>
            <p>紀錄總覽</p>
        </div>
        <div>
            <p style={{
                fontSize: "20px"
            }}>總金額: {totalAmount}</p>
            <p style={{
                fontSize: "18px",
                opacity: "0.5"
            }}>總筆數: {totalCount}</p>
        </div>
        <div style={{
            display: "flex",
            flexDirection: "column",
            alignItems: "flex-start",
            justifyContent: "flex-start",
            backgroundColor: "rgb(0, 0, 0)",
            width: "500px",
            height: "1000px"
        }}>
            {showRecordList}
        </div>
    </>
    );
}