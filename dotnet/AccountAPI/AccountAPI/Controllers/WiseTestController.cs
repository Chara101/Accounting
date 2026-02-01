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
        [HttpGet("records")]
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

        [HttpPost("record")]
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

        [HttpPut("record/{id:int:min(1)}")]
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
        [HttpDelete("record/{id:int:min(1)}")]
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

        [HttpPost("category")]
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

        [HttpDelete("category/{id:int:min(1)}")]
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

        [HttpPost("subcategory")]
        public IActionResult AddSubCategory([FromBody] int category_id, [FromQuery] string new_sub_name)
        {
            try
            {
                _db.AddSubCategory(category_id, new_sub_name); //warning 要對MssqlCtrl做修改 才能正確使用
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("subcategory/{id:int:min(1)}")]
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

        [HttpGet("categories")]
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
        [HttpGet("subcategories")]
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
