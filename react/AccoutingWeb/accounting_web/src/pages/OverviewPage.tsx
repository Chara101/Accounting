import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { 
  Card, CardContent, Typography, List, ListItem, 
  ListItemText, IconButton, Divider, Pagination, Box, Chip, Grid 
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import EditIcon from '@mui/icons-material/Edit';

import { getAllRecords, deleteRecord } from '../api/accountingService';
import type { AccountingRecord } from '../types';
import { groupRecordsByDate, calculateTotal } from '../utils/dataHelpers';

const ITEMS_PER_PAGE = 10;

const OverviewPage: React.FC = () => {
  const navigate = useNavigate();
  const [records, setRecords] = useState<AccountingRecord[]>([]);
  const [page, setPage] = useState(1);

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

  const totalPages = Math.ceil(records.length / ITEMS_PER_PAGE);
  const startIndex = (page - 1) * ITEMS_PER_PAGE;
  const currentData = records.slice(startIndex, startIndex + ITEMS_PER_PAGE);
  const groupedData = groupRecordsByDate(currentData);
  const totalAssets = calculateTotal(records);

  const handleDelete = async (record: AccountingRecord) => {
    if (window.confirm(`確定要刪除這筆 $${record.amount} 的紀錄嗎？`)) {
      try {
        await deleteRecord(record);
        fetchData(); 
      } catch (error) {
        alert('刪除失敗');
      }
    }
  };

  return (
    <Box sx={{ flexGrow: 1 }}>
      {/* RWD 佈局核心: Grid Container */}
      <Grid container spacing={3}>
        
        {/* 左側區塊：總資產卡片 */}
        {/* xs={12}: 手機佔滿整行 */}
        {/* md={4}: 電腦佔 1/3 寬度 */}
        <Grid size={{xs:12, md:4}}>
          <Card 
            elevation={3} 
            sx={{ 
              height: '100%', 
              display: 'flex', 
              flexDirection: 'column', 
              justifyContent: 'center', 
              textAlign: 'center', 
              py: { xs: 3, md: 5 }, // 手機版 padding 小一點，電腦版大一點
              bgcolor: '#e3f2fd' // 淺藍色背景
            }}
          >
            <Typography variant="h6" color="text.secondary" gutterBottom>
              目前總資產
            </Typography>
            <Typography 
              variant="h3" 
              color="primary" 
              sx={{ 
                fontWeight: 'bold',
                fontSize: { xs: '2.5rem', md: '3.5rem' } // 手機字體小一點
              }}
            >
              ${totalAssets.toLocaleString()}
            </Typography>
            <Typography variant="caption" sx={{ mt: 2, display: 'block' }}>
              統計至 {new Date().toLocaleDateString()}
            </Typography>
          </Card>
        </Grid>

        {/* 右側區塊：交易列表 */}
        {/* xs={12}: 手機佔滿整行 */}
        {/* md={8}: 電腦佔 2/3 寬度 */}
        <Grid size={{xs:12, md:8}}>
          
          {records.length === 0 && (
            <Typography align="center" color="text.secondary" sx={{ mt: 4 }}>
              目前沒有紀錄，請新增一筆！
            </Typography>
          )}

          {Object.keys(groupedData).map((date) => (
            <Card key={date} sx={{ mb: 2, border: '1px solid #e0e0e0', boxShadow: 'none' }}>
              <CardContent sx={{ 
                backgroundColor: '#f8f9fa', 
                py: 1, 
                display: 'flex', 
                justifyContent: 'space-between', 
                alignItems: 'center',
                '&:last-child': { pb: 1 }
              }}>
                <Typography variant="subtitle1" fontWeight="bold">{date}</Typography>
                <Chip label={`小計: $${calculateTotal(groupedData[date])}`} size="small" />
              </CardContent>
              
              <List dense disablePadding>
                {groupedData[date].map((record) => (
                  <React.Fragment key={record.id}>
                    <ListItem
                      secondaryAction={
                        <Box>
                          <IconButton onClick={() => navigate(`/edit/${record.id}`)} size="small">
                            <EditIcon fontSize="small" />
                          </IconButton>
                          <IconButton onClick={() => handleDelete(record)} size="small" color="error">
                            <DeleteIcon fontSize="small" />
                          </IconButton>
                        </Box>
                      }
                    >
                      <ListItemText
                        primary={
                          <Typography variant="body1" sx={{ fontWeight: 500 }}>
                             {record.category} 
                             <Typography component="span" variant="body2" color="text.secondary" sx={{ ml: 1 }}>
                               ({record.subcategory})
                             </Typography>
                          </Typography>
                        }
                        secondary={record.comment}
                      />
                      <Typography 
                        variant="body1" 
                        sx={{ 
                          mr: 2, 
                          fontWeight: 'bold', 
                          fontSize: { xs: '1rem', md: '1.1rem' } 
                        }}
                      >
                        ${record.amount}
                      </Typography>
                    </ListItem>
                    <Divider component="li" />
                  </React.Fragment>
                ))}
              </List>
            </Card>
          ))}

          {/* 分頁 */}
          {totalPages > 1 && (
            <Box sx={{ display: 'flex', justifyContent: 'center', mt: 3, mb: 2 }}>
              <Pagination 
                count={totalPages} 
                page={page} 
                onChange={(_, val) => setPage(val)} 
                color="primary" 
              />
            </Box>
          )}
        </Grid>
      </Grid>
    </Box>
  );
};

export default OverviewPage;