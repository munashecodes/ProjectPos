using Microsoft.AspNetCore.Mvc;
using ProjectPos.Services.DTOs;
using ProjectPos.Services.Interfaces;

namespace ProjectPos.Web.Controllers
{
    public class ExpenseController : Controller
    {
        //inject IExpenseService
        private readonly IExpenseService _service;
        public ExpenseController(IExpenseService service)
        {
            _service = service;
        }

        //add apis corresponding to IExpenseService methods
        [HttpPost("api/addExpense")]
        public async Task<ActionResult> Create([FromBody] ExpenseDto expense)
        {
            var res = await _service.CreateAsync(expense);
            return Ok(res);
        }

        [HttpPut("api/updateExpense")]
        public async Task<ActionResult> Update([FromBody] ExpenseDto expense)
        {
            var res = await _service.UpdateAsync(expense);
            return Ok(res);
        }

        [HttpDelete("api/deleteExpense/{id:int}/{userId:int}")]
        public async Task<ActionResult> Delete(int id, int userId)
        {
            var res = await _service.DeleteAsync(id, userId );
            return Ok(res);
        }

        [HttpGet("api/getAllExpenses")]
        public async Task<ActionResult> GetAll()
        {
            var res = await _service.GetAllAsync();
            return Ok(res);
        }

        [HttpGet("api/getExpense/{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var res = await _service.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpGet("api/getExpensesByAccountId/{accountId:int}")]
        public async Task<ActionResult> GetByAccountId(int accountId)
        {
            var res = await _service.GetByAccountAsync(accountId);
            return Ok(res);
        }

        [HttpGet("api/getExpensesByCompanyId/{companyId:int}")]
        public async Task<ActionResult> GetByCompanyId(int companyId)
        {
            var res = await _service.GetByCompanyAsync(companyId);
            return Ok(res);
        }

        //approve expense
        [HttpPut("api/approveExpense")]
        public async Task<ActionResult> Approve([FromBody] ExpenseDto expense)
        {
            var res = await _service.ApproveAsync(expense);
            return Ok(res);
        }
    }
}
