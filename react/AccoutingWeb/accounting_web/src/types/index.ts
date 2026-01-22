// 定義 API 回傳的帳單紀錄介面
export interface AccountingRecord {
  id: number;
  date: string; // ISO 格式或 YYYY-MM-DD
  category_id: number;
  category: string;
  subCategory_id: number;
  subcategory: string;
  amount: number;
  comment: string;
}

// 定義新增帳單時傳送的資料結構
export interface NewRecordPayload {
  category_id: number;
  subcategory_id: number;
  user_id: number;
  amount: number;
  comment: string;
}

// 定義科目
export interface Category {
  category_id: number;
  category: string;
}

// 定義子科目
export interface SubCategory {
  subcategory_id: number;
  subcategory: string;
}
// src/types/index.ts

// ... (保留原本的 AccountingRecord, NewRecordPayload 等)

// 新增：科目與子科目關聯表 (對應 API: GetAllCategoriesAndSub)
export interface CategoryRelation {
  category_id: number;
  subcategory_id: number;
}