# AccountAPI 使用指南
#### 請自己運行API，本人測試用路由 : https://localhost:7244/api/

| 目的  | http方法 | 請求 | 回傳 |
| --- | ------ | ---- | ---- |
| GetAllRecords | Get | 無 | 格式：json;欄位：id, date, category_id, category, subCategory_id, subcategory, amount, comment; | 
| GetRecord | Get | 格式：json;欄位：id(尋找相同id), date(同一天), category_id(同科目), subCategory_id(同子科目), amount(同金額); | 同GetAllRecords |
| GetRecordInRange| 同GetRecord | 同GetAllRecords |
| GetTotals | Get |
| GetTotalsBy
| GetTotalsInRange
| AddObject
| Renew
| Delete
| AddCategory
| DeleteCategory
| AddSubCategory
| DeleteSubCategory
| GetAllCategories
| GetAllSubCategories
| GetAllCategoriesAndSub

> [note]
> 問題：依賴後端篩選是否更花時間
> 如果是：改寫api，並使用前端處理
