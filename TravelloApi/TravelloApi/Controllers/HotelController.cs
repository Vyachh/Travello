using Bogus;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using TravelloApi.Interfaces;
using TravelloApi.Models;




namespace TravelloApi.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class HotelController : ControllerBase
  {
    private readonly IHotelRepository hotelRepository;

    public HotelController(IHotelRepository hotelRepository)
    {
      this.hotelRepository = hotelRepository;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>Возвращает полный список отелей из базы данных</returns>
    [HttpGet("getall")] //Client side search
    public async Task<IActionResult> GetAll()
    {
      return Ok(await hotelRepository.AsyncGetAll());
    }


    [HttpGet("FakeHotels")]
    public IActionResult AddFakeHotels()
    {
      Hotel[] hotels = new Hotel[]
{
    new Hotel
    {
        Title = "Luxury Oasis Resort & Spa",
        Description = "Experience the epitome of luxury and relaxation at our exquisite resort and spa.",
        Grade = 5.0,
        Price = 350.0,
        PictureUrl = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/186565392.jpg?k=6cfafab82c24efbf1f3853a5bb80a1ba0104345098b90613d321b98b9d9ab946&o=&hp=1",
        City = "Miami"
    },
    new Hotel
    {
        Title = "Coastal Haven Inn",
        Description = "Discover the charm of seaside living at our cozy and comfortable inn.",
        Grade = 4.0,
        Price = 180.0,
        PictureUrl="https://cf.bstatic.com/xdata/images/hotel/max1024x768/284073375.jpg?k=1455b5a26092649ab63c0c7f50ca775f4a9da50736a96b49b8f4b79e9bae51ac&o=&hp=1",
        City = "Malibu"
    },
    new Hotel
    {
        Title = "Serenity Heights Lodge",
        Description = "Retreat to the tranquility of our mountain lodge and embrace nature's beauty.",
        Grade = 4.5,
        Price = 280.0,
        PictureUrl = "https://extradivers-worldwide.com/images/DynArtwork/zoom-serenity-heights_587.jpg",
        City = "Aspen"
    },
    new Hotel
    {
        Title = "Urban Elegance Suites",
        Description = "Indulge in modern sophistication and urban luxury at our chic suites.",
        Grade = 4.0,
        Price = 220.0,
        PictureUrl ="https://ak-d.tripcdn.com/images/220m13000000tnbwf556B_R_960_660_R5_D.jpg",
        City = "New York"
    },
    new Hotel
    {
        Title = "Rustic Riverside Retreat",
        Description = "Experience rustic charm and peaceful riverside views at our idyllic retreat.",
        Grade = 3.5,
        Price = 150.0,
        PictureUrl = "https://s3.amazonaws.com/static-loghome/media/Miller-24_11868_2023-05-26_09-52.jpg",
        City = "Portland"
    },
    new Hotel
    {
        Title = "Majestic Castle Hotel",
        Description = "Live like royalty in a breathtaking castle surrounded by opulence and history.",
        Grade = 5.0,
        Price = 450.0,
        PictureUrl="https://media-cdn.tripadvisor.com/media/photo-s/09/23/8a/d1/majestic-palace-hotel.jpg",
        City = "Edinburgh"
    },
    new Hotel
    {
        Title = "Tranquil Forest Lodge",
        Description = "Immerse yourself in the serenity of the forest and unwind in pure tranquility.",
        Grade = 4.0,
        Price = 210.0,
        PictureUrl="https://cf.bstatic.com/xdata/images/hotel/max1024x768/236627450.jpg?k=f94056659360fc773b6267bcc616d3cc044c9e3abb2451661cce7612b28a26c9&o=&hp=1",
        City = "Vancouver"
    },
    new Hotel
    {
        Title = "Seaside Bliss Resort",
        Description = "Enjoy ultimate beachfront bliss and create unforgettable memories by the sea.",
        Grade = 4.5,
        Price = 290.0,
        PictureUrl="https://cf.bstatic.com/xdata/images/hotel/max1024x768/366042873.jpg?k=a07612292ca588eb17693fe34ee8c4baa9e8ba724d02ee2a7bef002047c8934f&o=&hp=1",
        City = "Honolulu"
    },
    new Hotel
    {
        Title = "Charming Countryside Cottage",
        Description = "Escape to a charming cottage in the countryside for a cozy and peaceful retreat.",
        Grade = 3.0,
        Price = 130.0,
        PictureUrl="https://cf.bstatic.com/xdata/images/hotel/max1024x768/417166484.jpg?k=7c52ac6e58c99bd684d50b9e84d6a45d652f490239fc8959f13b1fb6f4420c2d&o=&hp=1",
        City = "Cotswolds"
    },
    new Hotel
    {
        Title = "Grand Cityscape Hotel",
        Description = "Experience the grandeur of city living with stunning views of the urban skyline.",
        Grade = 4.0,
        Price = 240.0,
        PictureUrl="https://grand-hyatt-jakarta-hotel.nochi.com/data/Photos/OriginalPhoto/14002/1400282/1400282561/Grand-Hyatt-Jakarta-Hotel-Exterior.JPEG",
        City = "Dubai"
    }
};

      foreach (var item in hotels)
      {
        hotelRepository.Add(item);
      }
      return Ok();
    }
  }
}
