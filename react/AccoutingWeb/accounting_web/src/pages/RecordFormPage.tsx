import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { 
  TextField, Button, MenuItem, Select, FormControl, 
  InputLabel, Box, Typography, Paper 
} from '@mui/material';

import { 
  addRecord, renewRecord, getRecordById, 
  getAllCategories, getAllSubCategories 
} from '../api/accountingService';
import type { Category, SubCategory, AccountingRecord } from '../types';

const RecordFormPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const isEditMode = Boolean(id);

  const [formData, setFormData] = useState({
    date: new Date().toLocaleDateString('en-CA'), // 修正：使用本地時間格式 YYYY-MM-DD
    category_id: '' as unknown as number,
    subcategory_id: '' as unknown as number,
    amount: '' as unknown as number,
    comment: '',
    user_id: 1,
    id: 0,
    category: '',
    subcategory: ''
  });

  const [categories, setCategories] = useState<Category[]>([]);
  const [subCategories, setSubCategories] = useState<SubCategory[]>([]);

  useEffect(() => {
    const initData = async () => {
      try {
        const [cats, subs] = await Promise.all([
          getAllCategories(), 
          getAllSubCategories()
        ]);
        setCategories(cats);
        setSubCategories(subs);
        if (isEditMode && id) {
          const record = await getRecordById(Number(id));
          if (record) {
            setFormData({
              id: record.id,
              date: record.date.includes('T') ? record.date.split('T')[0] : record.date,
              amount: record.amount,
              comment: record.comment,
              category: record.category,
              category_id: record.category_id,
              subcategory_id: record.subcategory_id,
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
    
    const basePayload = {
      date: formData.date,
      category_id: Number(formData.category_id),
      subcategory_id: Number(formData.subcategory_id),
      amount: Number(formData.amount),
      comment: formData.comment,
    };

    try {
      if (isEditMode) {
        await renewRecord({
          ...basePayload,
          id: Number(id),
          user_id: formData.user_id,
          category: formData.category,
          subcategory: formData.subcategory
        } as unknown as AccountingRecord); 
      } else {
        await addRecord({
          ...basePayload,
          user_id: formData.user_id,
        });
      }
      navigate('/');
    } catch (error) {
      alert('儲存失敗');
    }
  };

  return (
    // RWD 調整重點：Paper 的 sx 屬性
    <Paper 
      elevation={3} 
      sx={{ 
        width: '100%',           // 預設 (手機) 佔滿寬度
        maxWidth: { sm: 500 },   // 螢幕大於 sm (600px) 時，最大寬度 500px
        mx: 'auto',              // 水平置中 (margin-left/right: auto)
        p: { xs: 2, md: 4 },     // 手機版內距 16px，電腦版 32px
        mt: 4,
        borderRadius: 2          // 圓角好看一點
      }}
    >
      <Typography variant="h5" align="center" gutterBottom sx={{ fontWeight: 'bold' }}>
        {isEditMode ? '修改紀錄' : '新增一筆消費'}
      </Typography>

      <Box component="form" onSubmit={handleSubmit} sx={{ display: 'flex', flexDirection: 'column', gap: 2.5 }}>
        
        <TextField
          label="日期"
          type="date"
          fullWidth
          InputLabelProps={{ shrink: true }}
          value={formData.date}
          onChange={(e) => setFormData({...formData, date: e.target.value})}
          required
        />

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
            {/* 未來可優化：根據 category_id 過濾顯示 */}
            {subCategories.map((s) => (
              <MenuItem key={s.subcategory_id} value={s.subcategory_id}>{s.subcategory ?? "錯誤"}</MenuItem>
            ))}
          </Select>
        </FormControl>

        <TextField
          label="金額"
          type="number"
          fullWidth
          inputProps={{ step: "0.01" }}
          value={formData.amount}
          onChange={(e) => setFormData({...formData, amount: Number(e.target.value)})}
          required
        />

        <TextField
          label="備註"
          fullWidth
          multiline
          rows={3}
          value={formData.comment}
          onChange={(e) => setFormData({...formData, comment: e.target.value})}
        />

        <Box sx={{ display: 'flex', gap: 2, mt: 1 }}>
          <Button 
            onClick={() => navigate('/')} 
            color="inherit" 
            fullWidth
            variant="outlined"
          >
            取消
          </Button>
          <Button 
            type="submit" 
            variant="contained" 
            fullWidth 
            size="large"
          >
            {isEditMode ? '儲存' : '新增'}
          </Button>
        </Box>
      </Box>
    </Paper>
  );
};

export default RecordFormPage;