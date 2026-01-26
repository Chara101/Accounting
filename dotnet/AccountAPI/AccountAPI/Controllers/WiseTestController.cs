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
        [HttpGet("search/GetAllRecord")]
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

        [HttpGet("search/GetRecord")]
        public IActionResult GetRecord([FromQuery] RecordForm r)
        {
            try
            {
                List<RecordForm> result = _db.GetRecordsBy(r);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("search/GetRecordInRange")]
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

        [HttpGet("search/GetAllTotals")]
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
        [HttpPost("search/GetTotalsBy")]
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
        [HttpPost("search/GetTotalsInRange")]
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

        [HttpPost("add/AddObject")]
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

        [HttpPut("renew")]
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
        [HttpDelete("delete")]
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

        [HttpPost("Category/add")]
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

        [HttpDelete("Category/delete")]
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

        [HttpPost("SubCategory/add")]
        public IActionResult AddSubCategory([FromBody] JsonElement data)
        {
            try
            {
                if (!data.TryGetProperty("category_id", out JsonElement cid) || !data.TryGetProperty("subcategory", out JsonElement sc_name))
                _db.AddSubCategory(cid, sc_name);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("SubCategory/delete")]
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

        [HttpGet("Category/getAll")]
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
        [HttpGet("SubCategory/getAll")]
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
        [HttpGet("CategoryAndSub/getAll")]
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
