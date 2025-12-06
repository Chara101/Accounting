// /src/components/RecordList.tsx

import React, { useEffect, useState } from 'react';
import { getAllRecords } from '../api/accountingService';
import type { AccountingRecord } from '../types';

const RecordList: React.FC = () => {
  const [records, setRecords] = useState<AccountingRecord[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchRecords = async () => {
      try {
        setLoading(true);
        const data = await getAllRecords();
        setRecords(data);
        setError(null);
      } catch (err) {
        setError('載入帳單紀錄失敗，請檢查 API 伺服器。');
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    fetchRecords();
  }, []);

  if (loading) return <div>載入中...</div>;
  if (error) return <div style={{ color: 'red' }}>錯誤: {error}</div>;

  return (
    <section>
      <h2>記帳紀錄列表 📋</h2>
      {records.length === 0 ? (
        <p>目前沒有任何紀錄。</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>日期</th>
              <th>科目</th>
              <th>子科目</th>
              <th>金額</th>
              <th>備註</th>
            </tr>
          </thead>
          <tbody>
            {records.map((record) => (
              <tr key={record.id}>
                <td>{new Date(record.date).toLocaleDateString()}</td>
                <td>{record.category}</td>
                <td>{record.subcategory}</td>
                <td>${record.amount.toFixed(2)}</td>
                <td>{record.comment}</td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </section>
  );
};

export default RecordList;