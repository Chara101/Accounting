import type { AccountingRecord } from '../types';

// 定義分組後的資料結構：Key 是日期字串，Value 是該日期的紀錄陣列
export interface GroupedRecords {
  [date: string]: AccountingRecord[];
}

/**
 * 將帳單紀錄列表按日期分組，並按日期降序排列 (最新的日期在上面)
 */
export const groupRecordsByDate = (records: AccountingRecord[]): GroupedRecords => {
  // 1. 複製並排序 (防止更動到原陣列)
  const sortedRecords = [...records].sort((a, b) => {
    return new Date(b.date).getTime() - new Date(a.date).getTime();
  });

  // 2. 進行分組 reduce
  return sortedRecords.reduce((groups, record) => {
    // 假設 date 格式為 "2023-10-27T00:00:00"，我們只取 "2023-10-27"
    const dateKey = record.date.split('T')[0];
    
    if (!groups[dateKey]) {
      groups[dateKey] = [];
    }
    groups[dateKey].push(record);
    return groups;
  }, {} as GroupedRecords);
};

/**
 * 計算傳入紀錄陣列的總金額
 */
export const calculateTotal = (records: AccountingRecord[]): number => {
  return records.reduce((sum, record) => sum + record.amount, 0);
};