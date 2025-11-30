import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import RecordList from "./RecordList.jsx";
import Record from "./Record.jsx";
import {createBrowserRouter, RouterProvider, Link} from "react-router-dom";

const router = createBrowserRouter([
  {
    path: "/",
    element: 
    <div style={{
      display: "flex",
      flexDirection: "column"
    }}>
      <Link to="/App">Go to App Page.</Link>
      <Link to="/RecordList">Go to RecordList Page.</Link>
    </div>
  },
  {
    path: "/App",
    element: 
    <div>
      <Link to="/">Back.</Link>
      <App/>
    </div>
  },
  {
    path: "/RecordList",
    element: (<RecordList/>)
  }
  ,{
    path: "/Record/:id",
    element: (<Record/>)
  }
]);
createRoot(document.getElementById("root")).render(<RouterProvider router={router}/>);
