// /src/types/index.ts

/**
 * 帳單紀錄的資料結構 (來自 GetAllRecords 的回傳)
 */
export interface AccountingRecord {
  id: number;
  date: string; // 建議使用 string 或 Date 類型，具體取決於後端格式
  category_id: number;
  category: string;
  subCategory_id: number;
  subcategory: string;
  amount: number;
  comment: string;
}

/**
 * 新增帳單所需的資料結構 (給 AddObject 的請求)
 */
export interface NewRecordPayload {
  category_id: number;
  subcategory_id: number;
  user_id: number; // 假設 user_id 也是必需的
  amount: number;
  comment: string;
}

/**
 * 科目資料結構 (來自 GetAllCategories 的回傳)
 */
export interface Category {
  category_id: number;
  category: string;
}

/**
 * 子科目資料結構 (來自 GetAllSubCategories 的回傳)
 */
export interface SubCategory {
  subcategory_id: number;
  subcategory: string;
}