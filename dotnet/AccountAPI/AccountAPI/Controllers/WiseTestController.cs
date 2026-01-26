using System.Data.Common;
using AccountAPI.DataStorage;
using AccountAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TestAcounting.DataStorage;
using System.Text.Json;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WiseTestController : ControllerBase
    {
        IDataStorage _db = new MssqlCtrl();
        // GET: api/<WiseTestController>
        [HttpGet("records/all")]
        public IActionResult GetAllRecord()
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                result = _db.GetAllRecords();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("records/match")]
        public IActionResult GetRecord([FromQuery] JsonElement data)
        {
            try
            {
                if(!data.TryGetProperty("Id", out JsonElement eid) 
                    && !data.TryGetProperty("date", out JsonElement edate) 
                    && !data.TryGetProperty("category_id", out JsonElement ecid)
                    && !data.TryGetProperty("subcategory_id", out JsonElement escid)
                    && !data.TryGetProperty("user_id", out JsonElement euid)
                    && !data.TryGetProperty("amount", out JsonElement eamount))
                    throw new ArgumentException("At least one search parameter must be provided.");
                RecordForm r = new RecordForm //warning 要把jsonElement轉換與加入
                {
                    Id = eid.ValueKind == JsonValueKind.Number? eid.GetInt32() : 0,
                    Date = edate.GetDateTime(),
                    Category_id = ecid.Get
                };
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public IActionResult GetRecord([FromQuery] RecordForm r)
        //{
        //    try
        //    {
        //        List<RecordForm> result = _db.GetRecordsBy(r);
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost("records/match/range")]
        public IActionResult GetRecordInRange([FromBody] BandRecord r)
        {
            try
            {
                List<RecordForm>  result = _db.GetRecordsBy(r.r1, r.r2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("totals/all")]
        public IActionResult GetTotals()
        {
            try
            {
                List<RecordForm> result = _db.GetAllTotals();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("totals/match")]
        public IActionResult GetTotalsBy([FromBody] RecordForm r)
        {
            try
            {
                List<RecordForm> result = _db.GetTotals(r);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("totals/match/range")]
        public IActionResult GetTotalsInRange([FromBody] BandRecord r)
        {
            try
            {
                List<RecordForm> result = _db.GetTotals(r.r1, r.r2);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("record/add")]
        public IActionResult AddObject([FromBody] RecordForm value)
        {
            try
            {
                _db.Add(value);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("record/renew")]
        public IActionResult Renew([FromBody] BandRecord r)
        {
            try
            {
                _db.Update(r.r1, r.r2);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<WiseTestController>/5
        [HttpDelete("record/delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                _db.Remove(new RecordForm() { Id = id });
                return Ok("sucess deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("category/add")]
        public IActionResult AddCategory([FromBody] JsonElement data)
        {
            try
            {
                if (data.TryGetProperty("category", out JsonElement categoryElement))
                {
                    string category = categoryElement.GetString() ?? string.Empty;
                    _db.AddCategory(category);
                }
                else throw new ArgumentException("Missing 'category' property in JSON data.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("category/delete")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                _db.RmCategory(id);
                return Ok("sucess deleted");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("subcategory/add")]
        public IActionResult AddSubCategory([FromBody] JsonElement data)
        {
            try
            {
                if (!data.TryGetProperty("category_id", out JsonElement cid) 
                    && !data.TryGetProperty("subcategory", out JsonElement sc_name))
                    throw new ArgumentException("Missing 'category_id' or 'subcategory' property in JSON data.");
                _db.AddSubCategory(cid, sc_name); //warning 要對MssqlCtrl做修改 才能正確使用
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("subcategory/delete")]
        public IActionResult DeleteSubCategory(int id)
        {
            try
            {
                _db.RmSubCategory(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("categories/all")]
        public IActionResult GetAllCategories()
        {
            try
            {
                List<RecordForm> result = new List<RecordForm>();
                result = _db.GetAllCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("subcategories/all")]
        public IActionResult GetAllSubCategories()
        {
            try
            {
                List<RecordForm> result = new List<RecordForm>();
                result = _db.GetAllSubCategories();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("relations/categoryAndSub")]
        public IActionResult GetAllCategoriesAndSub()
        {
            try
            {
                List<RecordForm> result = new List<RecordForm>();
                result = _db.GetAllCategoriesAndSub();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
