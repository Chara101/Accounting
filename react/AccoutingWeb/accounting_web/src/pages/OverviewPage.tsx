import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Card, CardContent, Typography, List, ListItem, 
  ListItemText, IconButton, Divider, Pagination, Box, Chip 
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';

// 引入 Part 1 寫好的模組
import { getAllRecords, deleteRecord } from '../api/accountingService';
import type { AccountingRecord } from '../types';
import { groupRecordsByDate, calculateTotal } from '../utils/dataHelpers';

const ITEMS_PER_PAGE = 10;

const OverviewPage: React.FC = () => {
  const navigate = useNavigate();
  const [records, setRecords] = useState<AccountingRecord[]>([]);
  const [page, setPage] = useState(1);

  // 1. 初始化載入資料
  const fetchData = async () => {
    try {
      const data = await getAllRecords();
      setRecords(data);
    } catch (error) {
      console.error('載入失敗:', error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  // 2. 計算分頁數據
  const totalPages = Math.ceil(records.length / ITEMS_PER_PAGE);
  const startIndex = (page - 1) * ITEMS_PER_PAGE;
  // 取出「當前頁面」要顯示的 10 筆資料
  const currentData = records.slice(startIndex, startIndex + ITEMS_PER_PAGE);

  // 3. 將當前頁面的資料進行「按日分組」
  const groupedData = groupRecordsByDate(currentData);
  
  // 計算總資產 (所有載入資料的總和)
  const totalAssets = calculateTotal(records);

  // 4. 刪除處理
  const handleDelete = async (record: AccountingRecord) => {
    if (window.confirm(`確定要刪除這筆 $${record.amount} 的紀錄嗎？`)) {
      try {
        await deleteRecord(record);
        // 刪除後重新抓取資料
        fetchData(); 
      } catch (error) {
        alert('刪除失敗，請檢查 API 連線');
      }
    }
  };

  return (
    <Box>
      {/* 頂部總金額區塊 */}
      <Box sx={{ textAlign: 'center', mb: 4, mt: 2 }}>
        <Typography variant="h6" color="text.secondary">目前總覽</Typography>
        <Typography variant="h3" color="primary" sx={{ fontWeight: 'bold' }}>
          ${totalAssets.toLocaleString()}
        </Typography>
      </Box>

      {/* 如果沒有資料顯示提示 */}
      {records.length === 0 && (
        <Typography align="center" color="text.secondary">目前沒有紀錄，請新增一筆！</Typography>
      )}

      {/* 顯示分組列表 */}
      {Object.keys(groupedData).map((date) => (
        <Card key={date} sx={{ mb: 2, border: '1px solid #e0e0e0', boxShadow: 'none' }}>
          {/* 卡片標題：日期與當日總和 */}
          <CardContent sx={{ 
            backgroundColor: '#f8f9fa', 
            py: 1, 
            display: 'flex', 
            justifyContent: 'space-between', 
            alignItems: 'center',
            '&:last-child': { pb: 1 }
          }}>
            <Typography variant="subtitle1" sx={{ fontWeight: 'bold' }}>{date}</Typography>
            <Chip label={`小計: $${calculateTotal(groupedData[date])}`} size="small" color="default" />
          </CardContent>
          
          <Divider />

          {/* 該日期的紀錄清單 */}
          <List dense disablePadding>
            {groupedData[date].map((record) => (
              <React.Fragment key={record.id}>
                <ListItem
                  secondaryAction={
                    <>
                      <IconButton edge="end" onClick={() => navigate(`/edit/${record.id}`)} sx={{ mr: 1 }}>
                        <EditIcon color="action" />
                      </IconButton>
                      <IconButton edge="end" onClick={() => handleDelete(record)}>
                        <DeleteIcon color="error" />
                      </IconButton>
                    </>
                  }
                >
                  <ListItemText
                    primary={
                      <Typography variant="body1" component="span" sx={{ fontWeight: 500 }}>
                         {record.category} <Typography component="span" variant="body2" color="text.secondary">({record.subcategory})</Typography>
                      </Typography>
                    }
                    secondary={record.comment || '(無備註)'}
                  />
                  <Typography variant="h6" sx={{ mr: 2, color: record.amount < 0 ? 'red' : 'green' }}>
                    ${record.amount}
                  </Typography>
                </ListItem>
                <Divider component="li" />
              </React.Fragment>
            ))}
          </List>
        </Card>
      ))}

      {/* 分頁控制器 */}
      {totalPages > 1 && (
        <Box sx={{ display: 'flex', justifyContent: 'center', my: 4 }}>
          <Pagination 
            count={totalPages} 
            page={page} 
            onChange={(_, val) => setPage(val)} 
            color="primary" 
            size="large"
          />
        </Box>
      )}
    </Box>
  );
};

export default OverviewPage;