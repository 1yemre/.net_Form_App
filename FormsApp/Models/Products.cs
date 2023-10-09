using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormsApp.Models
{

   //[Bind("Name","Price")]
    public class Product
    {
         //[BindNever] Butun alanlar set edilir ama product edilmez
         [Display(Name ="Urun id")]
          public int ProductId{get;set;}
          [Required(ErrorMessage ="Gerekli Bir Alan")]
          [StringLength(100)]
          [Display(Name ="Urun Adı")]
          public string?  Name{get;set;}
          [Required]
          [Range(0,100000)]
          [Display(Name ="Fiyat")]
          public decimal? Price{get;set;}
         
          [Display(Name ="Resim")]
          public string? Image{get;set;}=string.Empty;
          public bool IsActive { get; set;}
          [Display(Name ="Category")]
          [Required]
          public int? CategoryId {get;set;}

       

          


    }
   

}