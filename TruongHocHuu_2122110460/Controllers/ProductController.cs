using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TruongHocHuu_2122110460.Data;
using TruongHocHuu_2122110460.Model;
using TruongHocHuu_2122110460.Service;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using TruongHocHuu_2122110460.Dto;
using Microsoft.AspNetCore.Authorization;

namespace TruongHocHuu_2122110460.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Index()
        {
            return await _context.Products.ToListAsync();
        }
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(long categoryId)
        {
            var products = await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();

            if (products == null || !products.Any())
            {
                return NotFound("Không có sản phẩm trong danh mục này");
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return product;
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Vui lòng chọn ảnh sản phẩm.");
            }

            // Tạo tên file duy nhất
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            var filePath = Path.Combine(_environment.WebRootPath, "uploads", "products", fileName);

            // Tạo thư mục nếu chưa có
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

            // Lưu file vào thư mục
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Tạo entity sản phẩm
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                CategoryId = dto.CategoryId,
                Image = fileName,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm sản phẩm thành công", productId = product.Id });
        }
        [HttpGet("page")]
        public async Task<IActionResult> GetPagedProducts(int page = 1, int size = 5)
        {
            if (page < 1 || size < 1)
                return BadRequest("Page và Size phải > 0");

            var query = _context.Products
                .Include(p => p.Category);

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)size);

            var products = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return Ok(new
            {
                data = products,
                pagination = new
                {
                    currentPage = page,
                    pageSize = size,
                    totalItems,
                    totalPages
                }
            });
        }

        //[HttpPost]
        //public async Task<ActionResult<Product>> CreateProduct(Product product)
        //{
        //    _context.Products.Add(product);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        //}

        // ✅ Chỉnh sửa ở đây

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(long id, [FromForm] ProductCreateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Sản phẩm không tồn tại.");
            }

            // Nếu có ảnh mới -> xóa ảnh cũ & lưu ảnh mới
            if (dto.File != null && dto.File.Length > 0)
            {
                // Xóa ảnh cũ nếu tồn tại
                var oldImagePath = Path.Combine(_environment.WebRootPath, "uploads", "products", product.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                // Lưu ảnh mới
                var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
                var newFilePath = Path.Combine(_environment.WebRootPath, "uploads", "products", newFileName);

                Directory.CreateDirectory(Path.GetDirectoryName(newFilePath)!);

                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await dto.File.CopyToAsync(stream);
                }

                product.Image = newFileName;
            }

            // Cập nhật thông tin khác
            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.IsAvailable = dto.IsAvailable;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            return Ok("Cập nhật sản phẩm thành công.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }

    // ✅ Tạo DTO để Swagger xử lý file + form
    public class UploadProductImageDto
    {
        public long ProductId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
