import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  TextField, Button, MenuItem, Select, FormControl, 
  InputLabel, Box, Typography, Paper 
} from '@mui/material';

// 引入 API 與型別
import { 
  addRecord, renewRecord, getRecordById, 
  getAllCategories, getAllSubCategories 
} from '../api/accountingService';
import type { Category, SubCategory, AccountingRecord } from '../types';

const RecordFormPage: React.FC = () => {
  const { id } = useParams<{ id: string }>(); // 取得 URL 參數
  const navigate = useNavigate();
  const isEditMode = Boolean(id);

  // 表單狀態
  const [formData, setFormData] = useState({
    date: new Date().toISOString().split('T')[0], // 預設今天 (YYYY-MM-DD)
    category_id: '' as unknown as number,
    subcategory_id: '' as unknown as number,
    amount: '' as unknown as number,
    comment: '',
    user_id: 1, // 預設使用者 ID
    // 以下欄位用於編輯模式的回填與 API 需求
    id: 0,
    category: '',
    subcategory: ''
  });

  // 下拉選單選項
  const [categories, setCategories] = useState<Category[]>([]);
  const [subCategories, setSubCategories] = useState<SubCategory[]>([]);

  // 載入初始資料
  useEffect(() => {
    const initData = async () => {
      try {
        // 平行載入科目與子科目
        const [cats, subs] = await Promise.all([
          getAllCategories(), 
          getAllSubCategories()
        ]);
        setCategories(cats);
        setSubCategories(subs);

        // 如果是編輯模式，載入該筆紀錄
        if (isEditMode && id) {
          const record = await getRecordById(Number(id));
          if (record) {
            // ✅ 修改後的寫法：明確指定欄位對應
            setFormData({
            id: record.id,
            date: record.date.includes('T') ? record.date.split('T')[0] : record.date,
            amount: record.amount,
            comment: record.comment,
            category: record.category,
            category_id: record.category_id,
            
            // 關鍵修正：將後端的 subCategory_id 對應給前端 state 的 subcategory_id
            subcategory_id: record.subCategory_id, 
            subcategory: record.subcategory,
            
            user_id: 1
            });
          }
        }
      } catch (error) {
        console.error('初始化失敗:', error);
      }
    };
    initData();
  }, [id, isEditMode]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    // 整理要傳送的資料
    const basePayload = {
      date: formData.date,
      category_id: Number(formData.category_id),
      subcategory_id: Number(formData.subcategory_id),
      amount: Number(formData.amount),
      comment: formData.comment,
    };

    try {
      if (isEditMode) {
        // 呼叫更新 API (Renew)
        // 注意：這裡假設 Renew API 需要完整的 AccountingRecord 結構
        await renewRecord({
          ...basePayload,
          id: Number(id),
          user_id: formData.user_id, // 雖然 TypeScript 介面沒定義 user_id 在 AccountingRecord，但有些後端需要
          category: formData.category,
          subcategory: formData.subcategory
        } as unknown as AccountingRecord); 
      } else {
        // 呼叫新增 API (AddObject)
        await addRecord({
          ...basePayload,
          user_id: formData.user_id,
        });
      }
      // 成功後跳轉回首頁
      navigate('/');
    } catch (error) {
      alert('儲存失敗，請檢查 API');
    }
  };

  return (
    <Paper elevation={3} sx={{ maxWidth: 500, mx: 'auto', p: 4, mt: 4 }}>
      <Typography variant="h5" align="center" gutterBottom>
        {isEditMode ? '修改紀錄' : '新增一筆消費'}
      </Typography>

      <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
        
        {/* 日期欄位 */}
        <TextField
          label="日期"
          type="date"
          fullWidth
          InputLabelProps={{ shrink: true }}
          value={formData.date}
          onChange={(e) => setFormData({...formData, date: e.target.value})}
          required
        />

        {/* 科目下拉選單 */}
        <FormControl fullWidth required>
          <InputLabel>科目</InputLabel>
          <Select
            value={formData.category_id}
            label="科目"
            onChange={(e) => {
              const catId = Number(e.target.value);
              const catName = categories.find(c => c.category_id === catId)?.category || '';
              setFormData({...formData, category_id: catId, category: catName});
            }}
          >
            {categories.map((c) => (
              <MenuItem key={c.category_id} value={c.category_id}>{c.category}</MenuItem>
            ))}
          </Select>
        </FormControl>

        {/* 子科目下拉選單 */}
        <FormControl fullWidth required>
          <InputLabel>子科目</InputLabel>
          <Select
            value={formData.subcategory_id}
            label="子科目"
            onChange={(e) => {
              const subId = Number(e.target.value);
              const subName = subCategories.find(s => s.subcategory_id === subId)?.subcategory || '';
              setFormData({...formData, subcategory_id: subId, subcategory: subName});
            }}
          >
            {/* MVP 簡化：這裡顯示所有子科目，理想情況應根據 category_id 過濾 */}
            {subCategories.map((s) => (
              <MenuItem key={s.subcategory_id} value={s.subcategory_id}>{s.subcategory}</MenuItem>
            ))}
          </Select>
        </FormControl>

        {/* 金額欄位 */}
        <TextField
          label="金額"
          type="number"
          fullWidth
          inputProps={{ step: "0.01" }} // 允許小數點
          value={formData.amount}
          onChange={(e) => setFormData({...formData, amount: Number(e.target.value)})}
          required
        />

        {/* 備註欄位 */}
        <TextField
          label="備註"
          fullWidth
          multiline
          rows={3}
          value={formData.comment}
          onChange={(e) => setFormData({...formData, comment: e.target.value})}
        />

        {/* 送出按鈕 */}
        <Button 
          type="submit" 
          variant="contained" 
          size="large" 
          sx={{ mt: 2, height: 50, fontSize: '1.1rem' }}
        >
          {isEditMode ? '儲存修改' : '新增紀錄'}
        </Button>

        {/* 取消按鈕 */}
        <Button onClick={() => navigate('/')} color="inherit">
          取消
        </Button>
      </Box>
    </Paper>
  );
};

export default RecordFormPage;