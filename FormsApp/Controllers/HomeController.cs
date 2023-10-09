using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
      public HomeController()
      {

      }
    public IActionResult Index(string searchString , string category)
    {
        var products =Repository.products;

        if(!string.IsNullOrEmpty(searchString))
        {
            ViewBag.Search=searchString;
            products=products.Where(p=>p.Name!.ToLower().Contains(searchString)).ToList();

        }
    
        if(!string.IsNullOrEmpty(category)&& category!="0")
        {
            products=products.Where(p =>p.CategoryId==int.Parse(category)).ToList();
        }

        //ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name",category);

        var model =new ProductViewModel{
            Products=products,
            Categories=Repository.Categories,
            SelectedCategory=category
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product model,IFormFile imagefile)
    {  
         var extension="";
         if(imagefile !=null )
         {
           var allowExtensions=new[]{".jpg",".jpeg",".png"};
            extension=Path.GetExtension(imagefile.FileName);//abc.jpg
            if(!allowExtensions.Contains(extension))
            {
                  ModelState.AddModelError("","Please choose a valid image!");
            }
         }
         if(ModelState.IsValid)
         {
           if(imagefile!=null)
           {
                var randomFileName=string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                    using(var Stream=new FileStream(path,FileMode.Create))
                    {
                    await imagefile!.CopyToAsync(Stream);
                    }
                        model.Image=randomFileName;
                        model.ProductId=Repository.products.Count+1;
                        Repository.CreateProduct(model);
                        return RedirectToAction("Index");
           }
           
         }
        ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
    }

   public IActionResult Edit(int? id)
   {

        if(id==null)
        {
            return NotFound();
        }
        var entity=Repository.products.FirstOrDefault(p=>p.ProductId==id);
        if(entity==null)
        {
              return NotFound();
        }
        ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(entity);

   }
   [HttpPost]
  public async Task<IActionResult>Edit(int id,Product model,IFormFile? imagefile)
    {   
       if(id !=model.ProductId)
       {
         return NotFound();
       }

        if(ModelState.IsValid)
        {

             if(imagefile!=null)
             {
                 var extension=Path.GetExtension(imagefile.FileName);//abc.jpg
                var randomFileName=string.Format($"{Guid.NewGuid().ToString()}{extension}");
                var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);



                    using(var Stream=new FileStream(path,FileMode.Create))
                    {
                    await imagefile!.CopyToAsync(Stream);
                    }

                
                 model.Image=randomFileName;
             }
             Repository.EditProduct(model);
             return RedirectToAction("Index");
         }

        ViewBag.Categories=new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);

     }


  public IActionResult Delete(int? id)
  {
     if(id==null)
     {
        return NotFound();
     }
    var entity=Repository.products.FirstOrDefault(p=>p.ProductId==id);
    if(entity==null)
    {
         return NotFound();
    }
    return View("DeleteConfirm",entity);
    
  }

  [HttpPost]
  public IActionResult Delete(int id, int ProductId)
  {
     if(id !=ProductId)
     {
         return NotFound();
     }
     var entity=Repository.products.FirstOrDefault(p=>p.ProductId== ProductId);
  
      if(entity==null)
      {
        return NotFound();
      }
 
    Repository.DeleteProduct(entity);
    return RedirectToAction("Index");

  }


  public IActionResult EditProduts(List<Product>Products)
  {
        foreach(var product in Products)
        {
             Repository.EditProduct(product);
        }
        return RedirectToAction("Index");
  }















}
 
    



