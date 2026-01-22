// src/App.tsx
import React from 'react';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import { AppBar, Toolbar, Typography, Button, Container, CssBaseline } from '@mui/material';
import AccountBalanceWalletIcon from '@mui/icons-material/AccountBalanceWallet';

// 引入頁面
import OverviewPage from './pages/OverviewPage';
import RecordFormPage from './pages/RecordFormPage';
import CategoryManagePage from './pages/CategoryManagePage'; // 1. 引入新頁面

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <CssBaseline />
      <AppBar position="static">
        <Toolbar>
          <AccountBalanceWalletIcon sx={{ mr: 2 }} />
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            記帳小幫手
          </Typography>
          
          <Button color="inherit" component={Link} to="/">總覽</Button>
          <Button color="inherit" component={Link} to="/add">記一筆</Button>
          {/* 2. 新增導覽按鈕 */}
          <Button color="inherit" component={Link} to="/categories">科目管理</Button> 
        </Toolbar>
      </AppBar>

      <Container maxWidth={false} sx={{ mt: 4, mb: 4 }} >
        <Routes>
          <Route path="/" element={<OverviewPage />} />
          <Route path="/add" element={<RecordFormPage />} />
          <Route path="/edit/:id" element={<RecordFormPage />} />
          {/* 3. 新增路由規則 */}
          <Route path="/categories" element={<CategoryManagePage />} />
        </Routes>
      </Container>
    </BrowserRouter>
  );
};

export default App;