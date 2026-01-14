import type { AccountingRecord, NewRecordPayload, Category, SubCategory } from '../types';

// 請確認您的後端 API 網址，若有不同請在此修改
const API_BASE_URL = 'https://localhost:7244/api';

/**
 * 通用的 fetch 封裝函式，處理 JSON 轉換與錯誤拋出
 */
async function apiFetch<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
  const url = `${API_BASE_URL}/${endpoint}`;
  
  try {
    const response = await fetch(url, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...options.headers,
      },
    });

    if (!response.ok) {
      throw new Error(`API Request Failed: ${response.status} ${response.statusText}`);
    }

    // 處理 204 No Content 或空回應的情況
    const text = await response.text();
    return text ? JSON.parse(text) : ({} as T);
  } catch (error) {
    console.error(`Error fetching ${endpoint}:`, error);
    throw error;
  }
}

// ==========================================
// 讀取類 API
// ==========================================

// 獲取所有帳單紀錄
export const getAllRecords = (): Promise<AccountingRecord[]> => {
  return apiFetch<AccountingRecord[]>('GetAllRecords');
};

// 獲取所有科目
export const getAllCategories = (): Promise<Category[]> => {
  return apiFetch<Category[]>('GetAllCategories');
};

// 獲取所有子科目
export const getAllSubCategories = (): Promise<SubCategory[]> => {
  return apiFetch<SubCategory[]>('GetAllSubCategories');
};

// 獲取單筆紀錄 (模擬：若後端 GetRecord 邏輯複雜，這裡我們先從全部資料中尋找)
// 注意：根據您的 API 表格，GetRecord 需要傳入很多參數尋找。
// 為了前端開發方便，我們這裡寫一個 helper function 從前端過濾 id。
export const getRecordById = async (id: number): Promise<AccountingRecord | undefined> => {
  const records = await getAllRecords();
  return records.find((r) => r.id === id);
};

// ==========================================
// 寫入類 API (CUD)
// ==========================================

// 新增帳單
export const addRecord = (payload: NewRecordPayload): Promise<void> => {
  return apiFetch<void>('AddObject', {
    method: 'POST',
    body: JSON.stringify(payload),
  });
};

// 更新帳單 (Renew)
// 根據 API 規格，需要傳入完整的物件
export const renewRecord = (record: AccountingRecord): Promise<void> => {
  return apiFetch<void>('Renew', {
    method: 'PUT',
    body: JSON.stringify(record),
  });
};

// 刪除帳單
// 根據 API 規格，Delete 是傳入 JSON Body 而非 URL 參數
export const deleteRecord = (record: AccountingRecord): Promise<void> => {
  return apiFetch<void>('Delete', {
    method: 'DELETE',
    body: JSON.stringify(record),
  });
};