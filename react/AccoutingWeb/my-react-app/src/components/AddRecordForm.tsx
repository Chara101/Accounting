// /src/components/AddRecordForm.tsx

import React, { useState } from 'react';
import { addRecord } from '../api/accountingService';
import type { NewRecordPayload } from '../types';

interface AddRecordFormProps {
  onRecordAdded: () => void; // 新增成功後回呼父元件刷新列表
}

const AddRecordForm: React.FC<AddRecordFormProps> = ({ onRecordAdded }) => {
  const [formData, setFormData] = useState<NewRecordPayload>({
    category_id: 1, // 暫時使用預設值，實際應從 API 獲取
    subcategory_id: 1, // 暫時使用預設值
    user_id: 1, // 暫時使用預設值
    amount: 0,
    comment: '',
  });
  const [submitStatus, setSubmitStatus] = useState<'idle' | 'loading' | 'success' | 'error'>('idle');

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: name === 'amount' ? parseFloat(value) || 0 : value,
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitStatus('loading');
    try {
      await addRecord(formData);
      setSubmitStatus('success');
      // 清空表單
      setFormData({ category_id: 1, subcategory_id: 1, user_id: 1, amount: 0, comment: '' }); 
      onRecordAdded(); // 通知父元件刷新列表
      setTimeout(() => setSubmitStatus('idle'), 3000);
    } catch (error) {
      setSubmitStatus('error');
      console.error('新增失敗:', error);
    }
  };

  return (
    <section>
      <h2>新增帳單 ✍️</h2>
      <form onSubmit={handleSubmit}>
        {/* 在實際應用中，category_id 和 subcategory_id 應該是下拉式選單 */}
        <div>
          <label>金額:</label>
          <input
            type="number"
            name="amount"
            value={formData.amount}
            onChange={handleChange}
            required
            step="0.01"
          />
        </div>
        <div>
          <label>備註:</label>
          <textarea
            name="comment"
            value={formData.comment}
            onChange={handleChange}
            required
          />
        </div>
        <div>
          {/* 簡化處理: 實際應是動態下拉選單 */}
          <label>科目 ID:</label>
          <input type="number" name="category_id" value={formData.category_id} onChange={handleChange} required />
        </div>
        <div>
          <label>子科目 ID:</label>
          <input type="number" name="subcategory_id" value={formData.subcategory_id} onChange={handleChange} required />
        </div>
        
        <button type="submit" disabled={submitStatus === 'loading'}>
          {submitStatus === 'loading' ? '提交中...' : '新增紀錄'}
        </button>

        {submitStatus === 'success' && <p style={{ color: 'green' }}>新增成功！</p>}
        {submitStatus === 'error' && <p style={{ color: 'red' }}>新增失敗，請重試。</p>}
      </form>
    </section>
  );
};

export default AddRecordForm;