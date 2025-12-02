# AccountAPI 使用指南
#### 請自己運行API，本人測試用路由 : https://localhost:7244/api/

| 目的  | http方法 | 請求 | 回傳 |
| --- | ------ | ---- | ---- |
| GetAllRecords | Get | 無 | 格式：json;欄位：id, date, category_id, category, subCategory_id, subcategory, amount, comment; | 
| GetRecord | Get | 格式：json;欄位：id(尋找相同id), date(同一天), category_id(同科目), subCategory_id(同子科目), amount(同金額); | 同GetAllRecords |
| GetRecordInRange | Get | 同GetRecord，但傳入成對的，一組代表一個區間 | 同GetAllRecords |
| GetTotals | Get | 無 | 格式：json; 欄位：id, date, category_id, subcategory_id, subCount, subAmount; 每一條代表同一天同科目子科被加入數量與總金額|
| GetTotalsBy | Get | 格式：json; 欄位：id, date, category_id, subcategory_id, count(帳單數） | 同GetTotals |
| GetTotalsInRange | Get | 同GetTotalsBy，但傳入成對的，一組代表一個區間 | 同GetTotals | 
| AddObject 加入新帳單 | Post | 格式：json; 欄位：category_id, subcategory_id, user_id, amount, comment; | 無 |
| Renew 修改指定帳單紀錄 | Put | 格式：json; 傳入要替換的目標資訊：id, date, category_id, subcategory_id, amount; 傳入要替換的資料：date, category_id, subcategory_id, amount, comment; | 無 |
| Delete 刪除指定紀錄 | Delete | 格式：json; 欄位：id, date, category_id, subcategory_id, amount; | 無 |
| AddCategory | Post | 格式：json; 欄位：category(name); | 無 |
| DeleteCategory | Delete | 格式：json; 欄位：category_id; | 無 |
| AddSubCategory | Post | 格式：json; 欄位：category_id, subcategory(name); | 無 |
| DeleteSubCategory | Delete | 格式：json; 欄位：subcategory_id; | 無 |
| GetAllCategories | Get | 無 | 格式：json; 欄位：category_id, category; |
| GetAllSubCategories | Get | 無 | 格式：json; 欄位：subcategory_id, subcategory; |
| GetAllCategoriesAndSub 科目與子科關係 | Get | 無 | 格式：json; 欄位：category_id, subcategory_id; |

> [note]
> 問題：依賴後端篩選是否更花時間
> 如果是：改寫api，並使用前端處理
