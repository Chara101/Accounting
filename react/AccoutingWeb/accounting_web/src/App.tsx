import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container, CssBaseline } from '@mui/material';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';

// 引入頁面
import OverviewPage from './pages/OverviewPage';
import RecordFormPage from './pages/RecordFormPage';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      {/* CSS Reset: 確保在不同瀏覽器顯示一致 */}
      <CssBaseline />
      
      {/* 導覽列 */}
      <AppBar position="static">
        <Toolbar>
          <AccountBalanceWalletIcon sx={{ mr: 2 }} />
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            記帳小幫手
          </Typography>
          
          {/* 導航按鈕 */}
          <Button color="inherit" component={Link} to="/">
            總覽
          </Button>
          <Button color="inherit" component={Link} to="/add" variant="outlined" sx={{ ml: 1, borderColor: 'white' }}>
            + 記一筆
          </Button>
        </Toolbar>
      </AppBar>

      {/* 主要內容區塊 */}
      <Container maxWidth="md" sx={{ mt: 4, mb: 4 }}>
        <Routes>
          {/* 1. 總覽頁面 (首頁) */}
          <Route path="/" element={<OverviewPage />} />
          
          {/* 2. 新增紀錄頁面 */}
          <Route path="/add" element={<RecordFormPage />} />
          
          {/* 3. 修改紀錄頁面 (帶 ID 參數) */}
          <Route path="/edit/:id" element={<RecordFormPage />} />
        </Routes>
      </Container>
    </BrowserRouter>
  );
};

export default App;