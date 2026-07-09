## Question

Add sorting and searching in the ui. search on different properties. search shows live search results. give complete code

## Answer

Replace `wwwroot/index.html` with this complete version.

```html



    
    
    
    


    

        

            
Product Management System

            
Minimal management UI backed by a layered ASP.NET Core API and in-memory persistence.



            
In-Memory Store
Retry
Circuit Breaker
Idempotency-Key

        


        

                
Create Product


                

                        
ProdName

                        

                    

                        
Price

                        

                    

                        
Brand

                        

                    

                        
Idempotency-Key

                        

                        

                            Used for POST only. If the same create request is retried, the API returns the same result instead of creating duplicates.
                        

                    
Save Product
Cancel Edit
Refresh


                

            

                
Products


                

                        
Search

                        
Type to search live...

                    

                        
Search In

                        
All Properties

                    

                        
Sort By

                        
ID

                    

                        
Direction

                        
Ascending

                    
Clear


                
Showing 0 of 0 products.

                

            

    


    


```

Notes:
- This adds **live search** using the `input` event.
- Search works on:
  - `ID`
  - `ProdName`
  - `Price`
  - `Brand`
  - or **All Properties**
- Sorting works on:
  - `ID`
  - `ProdName`
  - `Price`
  - `Brand`
- No API changes are required because the UI loads products once and filters/sorts them client-side.
