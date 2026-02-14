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
        IDataStorage _db;
        public WiseTestController()
        {
            _db = new MssqlCtrl();
        }
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

        [HttpGet("records/find")]
        public IActionResult GetRecord([FromQuery] Records_search_form data)
        {
            try
            {
                List<RecordForm> result = _db.GetRecordsBy(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("records/find/range")]
        public IActionResult GetRecordInRange([FromBody] Record_search_range_form data)
        {
            try
            {
                List<RecordForm>  result = _db.GetRecordsBy(data.edge1, data.edge2);
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
        public IActionResult GetTotalsBy([FromBody] Total_search_form data)
        {
            try
            {
                List<RecordForm> result = _db.GetTotals(data);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("totals/match/range")]
        public IActionResult GetTotalsInRange([FromBody] Total_search_range_form data)
        {
            try
            {
                List<RecordForm> result = _db.GetTotals(data.Form1, data.Form2);
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

        [HttpPut("record/{id:int:min(1)}/modify")]
        public IActionResult Renew(int id, [FromBody] RecordForm content)
        {
            try
            {
                _db.Update(new RecordForm { Id = id }, content);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<WiseTestController>/5
        [HttpDelete("record/{id:int:min(1)}/delete")]
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
        public IActionResult AddCategory([FromBody] string category)
        {
            try
            {
                _db.AddCategory(category);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("category/{id:int:min(1)}/delete")]
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
        public IActionResult AddSubCategory([FromBody] Subcategory_form sf)
        {
            try
            {
                _db.AddSubCategory(sf.Category_id, sf.Subcategory_name); //warning 要對MssqlCtrl做修改 才能正確使用
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("subcategory/{id:int:min(1)}/delete")]
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
        [HttpGet("categoryAndSub")]
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
