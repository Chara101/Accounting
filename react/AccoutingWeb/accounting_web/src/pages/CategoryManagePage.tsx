import React, { useEffect, useState } from 'react';
import { 
  Box, Typography, Paper, List, ListItem, ListItemText, 
  IconButton, TextField, Button, Divider, Collapse 
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import ExpandLess from '@mui/icons-material/ExpandLess';
import ExpandMore from '@mui/icons-material/ExpandMore';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';

import { 
  getAllCategories, getAllSubCategories, getAllRelations, 
  addCategory, deleteCategory, addSubCategory, deleteSubCategory 
} from '../api/accountingService';
import type { Category, SubCategory, CategoryRelation } from '../types';

const CategoryManagePage: React.FC = () => {
  // 資料狀態
  const [categories, setCategories] = useState<Category[]>([]);
  const [subCategories, setSubCategories] = useState<SubCategory[]>([]);
  const [relations, setRelations] = useState<CategoryRelation[]>([]);
  
  // UI 狀態：控制哪些科目是展開的 (Key: category_id, Value: boolean)
  const [openMap, setOpenMap] = useState<Record<number, boolean>>({});
  
  // 輸入框狀態
  const [newCategoryName, setNewCategoryName] = useState('');
  const [newSubNameMap, setNewSubNameMap] = useState<Record<number, string>>({}); // 每個科目都有自己的子科目輸入框

  // 1. 載入所有資料
  const fetchData = async () => {
    try {
      const [cats, subs, rels] = await Promise.all([
        getAllCategories(),
        getAllSubCategories(),
        getAllRelations()
      ]);
      setCategories(cats);
      setSubCategories(subs);
      setRelations(rels);
    } catch (error) {
      console.error('載入失敗', error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // 切換展開/收合
  const handleToggle = (id: number) => {
    setOpenMap((prev) => ({ ...prev, [id]: !prev[id] }));
  };

  // 處理新增主科目
  const handleAddCategory = async () => {
    if (!newCategoryName.trim()) return;
    try {
      await addCategory(newCategoryName);
      setNewCategoryName('');
      fetchData();
    } catch (error) {
      alert('新增科目失敗');
    }
  };

  // 處理刪除主科目
  const handleDeleteCategory = async (id: number) => {
    if (window.confirm('確定刪除此科目？底下的子科目關聯可能會受影響。')) {
      try {
        await deleteCategory(id);
        fetchData();
      } catch (error) {
        alert('刪除失敗');
      }
    }
  };

  // 處理新增子科目
  const handleAddSubCategory = async (categoryId: number) => {
    const name = newSubNameMap[categoryId];
    if (!name || !name.trim()) return;
    try {
      await addSubCategory(categoryId, name);
      setNewSubNameMap((prev) => ({ ...prev, [categoryId]: '' })); // 清空該輸入框
      fetchData();
    } catch (error) {
      alert('新增子科目失敗');
    }
  };

  // 處理刪除子科目
  const handleDeleteSubCategory = async (subId: number) => {
    if (window.confirm('確定刪除此子科目？')) {
      try {
        await deleteSubCategory(subId);
        fetchData();
      } catch (error) {
        alert('刪除失敗');
      }
    }
  };

  return (
    <Box sx={{ maxWidth: 800, mx: 'auto', mt: 4 }}>
      <Typography variant="h4" gutterBottom>科目管理</Typography>
      <Typography variant="body2" color="text.secondary" paragraph>
        在此管理您的記帳分類架構。點擊科目可展開查看子科目。
      </Typography>

      {/* 新增主科目區塊 */}
      <Paper sx={{ p: 2, mb: 3, display: 'flex', gap: 2, alignItems: 'center' }}>
        <TextField 
          label="新主科目名稱" 
          size="small" 
          fullWidth
          value={newCategoryName}
          onChange={(e) => setNewCategoryName(e.target.value)}
        />
        <Button 
          variant="contained" 
          startIcon={<AddCircleOutlineIcon />} 
          onClick={handleAddCategory}
          sx={{ whiteSpace: 'nowrap' }}
        >
          新增科目
        </Button>
      </Paper>

      {/* 科目列表 */}
      <Paper>
        <List>
          {categories.map((cat) => {
            // 找出此主科目底下的所有子科目 ID
            const relatedSubIds = relations
              .filter(r => r.category_id === cat.category_id)
              .map(r => r.subcategory_id);
            
            // 根據 ID 找出子科目物件
            const relatedSubs = subCategories.filter(s => relatedSubIds.includes(s.subcategory_id));
            const isOpen = openMap[cat.category_id];

            return (
              <React.Fragment key={cat.category_id}>
                {/* 主科目列 */}
                <ListItem
                  secondaryAction={
                    <IconButton edge="end" onClick={() => handleDeleteCategory(cat.category_id)} color="error">
                      <DeleteIcon />
                    </IconButton>
                  }
                  disablePadding
                  sx={{ bgcolor: '#f5f5f5' }}
                >
                  <IconButton onClick={() => handleToggle(cat.category_id)} sx={{ ml: 1 }}>
                    {isOpen ? <ExpandLess /> : <ExpandMore />}
                  </IconButton>
                  <ListItemText 
                    primary={cat.category} 
                    secondary={`${relatedSubs.length} 個子科目`} 
                    sx={{ cursor: 'pointer' }}
                    onClick={() => handleToggle(cat.category_id)}
                  />
                </ListItem>
                <Divider />

                {/* 子科目展開區 */}
                <Collapse in={isOpen} timeout="auto" unmountOnExit>
                  <List component="div" disablePadding>
                    {/* 新增子科目輸入列 */}
                    <ListItem sx={{ pl: 4, pr: 2, py: 2 }}>
                       <TextField
                          label={`在 ${cat.category} 下新增子科目`}
                          size="small"
                          fullWidth
                          variant="standard"
                          value={newSubNameMap[cat.category_id] || ''}
                          onChange={(e) => setNewSubNameMap({...newSubNameMap, [cat.category_id]: e.target.value})}
                       />
                       <Button onClick={() => handleAddSubCategory(cat.category_id)} sx={{ ml: 2 }}>
                         新增
                       </Button>
                    </ListItem>

                    {/* 子科目列表 */}
                    {relatedSubs.map(sub => (
                      <ListItem key={sub.subcategory_id} sx={{ pl: 4 }}>
                        <ListItemText primary={sub.subcategory} />
                        <IconButton size="small" onClick={() => handleDeleteSubCategory(sub.subcategory_id)}>
                          <DeleteIcon fontSize="small" color="disabled" />
                        </IconButton>
                      </ListItem>
                    ))}
                    {relatedSubs.length === 0 && (
                      <ListItem sx={{ pl: 4 }}>
                        <ListItemText secondary="暫無子科目" />
                      </ListItem>
                    )}
                  </List>
                  <Divider />
                </Collapse>
              </React.Fragment>
            );
          })}
        </List>
      </Paper>
    </Box>
  );
};

export default CategoryManagePage;