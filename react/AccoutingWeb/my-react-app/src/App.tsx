// /src/App.tsx

import React, { useState } from 'react';
import RecordList from './components/RecordList';
import AddRecordForm from './components/AddRecordForm';

const App: React.FC = () => {
  const [key, setKey] = useState(0); // 用來強制 RecordList 重新載入

  const handleRecordAdded = () => {
    // 當新增紀錄成功時，更新 key，讓 RecordList 重新載入數據
    setKey(prevKey => prevKey + 1);
  };

  return (
    <div style={{ padding: '20px', maxWidth: '800px', margin: '0 auto' }}>
      <h1>💸 React + TypeScript 記帳本</h1>
      <AddRecordForm onRecordAdded={handleRecordAdded} />
      <hr />
      {/* 使用 key 屬性來實現數據刷新 */}
      <RecordList key={key} /> 
    </div>
  );
};

export default App;