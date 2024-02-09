# Домашнее задание к Уроку 2. Работа с данными (CSV + статика), маппинг и кэширование
---
Доработайте контроллер, реализовав в нем метод возврата CSV-файла с товарами.
*Доработайте контроллер, реализовав статичный файл со статистикой работы кэш. Сделайте его доступным по ссылке.
*Перенесите строку подключения для работы с базой данных в конфигурационный файл приложения.

---
## РЕШЕНИЕ:
---

Для решения задачи  был  доработан контроллер ProductController.cs следующим образом:

1. В строках 66-79 реализован метод GetCsv(IEnumerable<Product> products), который  возвращает полученные данные  в строковом формате CSV:

```
        private string GetCsv(IEnumerable<Product> products)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var product in products)
            {
                sb.AppendLine($"{product.Name} " +
                              $"; {product.Price}" +
                              $" ; {product.Description}" +
                              $" ; {product.Category}\n");
            }

            return sb.ToString();
        }
```

2. В строках 81-99 реализован метод GetProductsCsv(), который возвращает CSV файл с данными из базы данных:

```
        [HttpGet]
        [Route("Csv")]
        public FileContentResult GetProductsCsv()
        {

            using (_context)
            {
                var products = _context.Products.Select(x =>
                                                    new Product
                                                    {
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Price = x.Price
                                                    }).ToList();

                var content = GetCsv(products);
                return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
            }
        }

```