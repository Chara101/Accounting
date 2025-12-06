// /src/api/accountingService.ts

import type { AccountingRecord, NewRecordPayload } from '../types';

const API_BASE_URL = 'https://localhost:7244/api'; // 請替換成您的實際 API 基礎 URL

/**
 * 處理通用的 fetch 請求
 * @param endpoint API 路由
 * @param options fetch 選項
 * @returns 回傳 JSON 資料
 */
async function apiFetch<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  const url = `${API_BASE_URL}/${endpoint}`;
  try {
    const response = await fetch(url, options);

    if (!response.ok) {
      // 處理 HTTP 錯誤狀態碼
      const errorData = await response.json().catch(() => ({ message: 'Unknown error' }));
      throw new Error(`API Error: ${response.status} - ${errorData.message || 'Server error'}`);
    }

    // 處理無回傳內容 (例如 POST, PUT, DELETE 成功時)
    if (response.status === 204 || response.headers.get('content-length') === '0') {
      return {} as T; // 回傳空物件或您定義的成功類型
    }

    return response.json() as Promise<T>;
  } catch (error) {
    console.error('Fetch error:', error);
    throw error;
  }
}

/**
 * [GET] 獲取所有記帳紀錄
 * @returns 帳單紀錄陣列
 */
export const getAllRecords = (): Promise<AccountingRecord[]> => {
  return apiFetch<AccountingRecord[]>('GetAllRecords');
};

/**
 * [POST] 加入新帳單
 * @param record 新帳單資料
 */
export const addRecord = (record: NewRecordPayload): Promise<void> => {
  const options: RequestInit = {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(record),
  };
  
  // 假設 AddObject 成功回傳 200 或 204
  return apiFetch<void>('AddObject', options); 
};

// ... 其他 API 函數如 getTotals, renewRecord, deleteRecord 等，都可在此處新增。