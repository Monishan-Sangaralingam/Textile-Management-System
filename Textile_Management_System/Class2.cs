using System;
using System.Collections;
using System.Collections.Generic;

namespace Textile_Management_System
{
    // 1. CustomDictionary
    public class CustomDictionary<K, V> : IEnumerable<KeyValuePair<K, V>>
    {
        private const int DefaultCapacity = 16;
        private const double LoadFactor = 0.75;

        private LinkedList<KeyValuePair<K, V>>[] _buckets;
        private int _count;

        public CustomDictionary()
        {
            _buckets = new LinkedList<KeyValuePair<K, V>>[DefaultCapacity];
            _count = 0;
        }

        // Hash function to map a key to an index
        private int GetBucketIndex(K key)
        {
            int hashCode = key.GetHashCode();
            return Math.Abs(hashCode % _buckets.Length);
        }

        // Insert or update a key-value pair
        public void Add(K key, V value)
        {
            if ((double)_count / _buckets.Length >= LoadFactor)
            {
                Resize();
            }

            int index = GetBucketIndex(key);

            if (_buckets[index] == null)
            {
                _buckets[index] = new LinkedList<KeyValuePair<K, V>>();
            }

            // Check if the key already exists
            foreach (var kvp in _buckets[index])
            {
                if (kvp.Key.Equals(key))
                {
                    throw new ArgumentException("An item with the same key already exists.");
                }
            }

            _buckets[index].AddLast(new KeyValuePair<K, V>(key, value));
            _count++;
        }

        // Get the value associated with a key
        public V Get(K key)
        {
            int index = GetBucketIndex(key);

            if (_buckets[index] != null)
            {
                foreach (var kvp in _buckets[index])
                {
                    if (kvp.Key.Equals(key))
                    {
                        return kvp.Value;
                    }
                }
            }

            throw new KeyNotFoundException("Key not found.");
        }

        // Check if the dictionary contains a key
        public bool ContainsKey(K key)
        {
            int index = GetBucketIndex(key);

            if (_buckets[index] != null)
            {
                foreach (var kvp in _buckets[index])
                {
                    if (kvp.Key.Equals(key))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        // Resize the buckets array when the load factor is exceeded
        private void Resize()
        {
            var oldBuckets = _buckets;
            _buckets = new LinkedList<KeyValuePair<K, V>>[_buckets.Length * 2];
            _count = 0;

            foreach (var bucket in oldBuckets)
            {
                if (bucket != null)
                {
                    foreach (var kvp in bucket)
                    {
                        Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        // Implement IEnumerable to support collection initializers
        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            foreach (var bucket in _buckets)
            {
                if (bucket != null)
                {
                    foreach (var kvp in bucket)
                    {
                        yield return kvp;
                    }
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // 2. DynamicArray
    public class DynamicArray
    {
        private object[] _items; // Internal array to store elements
        private int _count; // Number of elements in the array

        public DynamicArray(int initialCapacity = 4)
        {
            _items = new object[initialCapacity];
            _count = 0;
        }

        // Add an item to the array
        public void Add(object item)
        {
            if (_count == _items.Length)
            {
                // Double the array size when full
                Resize(_items.Length * 2);
            }
            _items[_count] = item;
            _count++;
        }

        // Resize the internal array
        private void Resize(int newSize)
        {
            object[] newArray = new object[newSize];
            Array.Copy(_items, newArray, _count);
            _items = newArray;
        }

        // Get the number of elements
        public int Count => _count;

        // Indexer to access elements
        public object this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                return _items[index];
            }
        }

        // Remove an item at a specific index
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _count)
                throw new IndexOutOfRangeException();

            for (int i = index; i < _count - 1; i++)
            {
                _items[i] = _items[i + 1];
            }
            _count--;
        }
    }

    
    // 3. Store
    public class Store
    {
        // Fields
        static CustomDictionary<string, int> Costomer = new CustomDictionary<string, int>
        {
            { "Gents", 1 },
            { "Ladies", 2 },
            { "Boys", 3 },
            { "Girls", 4 },
            { "Kids", 5 }
        };

        static CustomDictionary<string, int> Dtype = new CustomDictionary<string, int>
        {
            { "Shirt", 1 },
            { "T-Shirt", 2 },
            { "Trouser", 3 },
            { "Short", 4 },
            { "Sports Kit", 5 },
            { "Skirt", 6 },
            { "Blouse", 7 },
            { "Saree", 8 }
        };

        private List<object[]> MergeSort(List<object[]> list, Func<object[], object[], int> compare)
        {
            if (list.Count <= 1)
                return list;

            int mid = list.Count / 2;
            List<object[]> left = list.GetRange(0, mid);
            List<object[]> right = list.GetRange(mid, list.Count - mid);

            left = MergeSort(left, compare);
            right = MergeSort(right, compare);
            return Merge(left, right, compare);
        }

        private List<object[]> Merge(List<object[]> left, List<object[]> right, Func<object[], object[], int> compare)
        {
            List<object[]> result = new List<object[]>();
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (compare(left[i], right[j]) <= 0)
                {
                    result.Add(left[i]);
                    i++;
                }
                else
                {
                    result.Add(right[j]);
                    j++;
                }
            }

            while (i < left.Count)
            {
                result.Add(left[i]);
                i++;
            }

            while (j < right.Count)
            {
                result.Add(right[j]);
                j++;
            }

            return result;
        }

        // 5 Dynamic Arrays (Gents, Ladies, Boys, Girls, Kids)
        private static DynamicArray[] productArrays = new DynamicArray[5]
        {
            new DynamicArray(), // Gents
            new DynamicArray(), // Ladies
            new DynamicArray(), // Boys
            new DynamicArray(), // Girls
            new DynamicArray()  // Kids
        };

        // Methods
        public void MStore()
        {
            Console.WriteLine("\t\t--------------------------------");
            Console.WriteLine("\t\t|      STORE ADMINISTRATION     |");
            Console.WriteLine("\t\t---------------------------------");

            Console.WriteLine("\n\t\t 01. Add new product ");
            Console.WriteLine("\n\t\t 02. Edit the product");
            Console.WriteLine("\n\t\t 03. Delete The Product");
            Console.WriteLine("\n\t\t 04. Check the Stock");
            Console.WriteLine("\n\t\t 05. View All Products");
            Console.WriteLine("\n\t\t 06. Main Menu");

            Console.Write("Enter Your Choice: ");
            if (int.TryParse(Console.ReadLine(), out int y))
            {
                switch (y)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        EditProduct();
                        break;
                    case 3:
                        DeleteProduct();
                        break;
                    case 4:
                        CategorizeProduct();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("Enter the valid input");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Wrong Input");
            }
        }

        public void AddProduct()
        {
            // Temporary array to store the product code
            int[] tempCode = new int[10];

            // Step 1: Get customer category
            Console.Write("Enter the Category (Gents, Ladies, Boys, Girls, Kids): ");
            string selectedProduct = Console.ReadLine();

            if (Costomer.ContainsKey(selectedProduct))
            {
                int D12 = Costomer.Get(selectedProduct);
                tempCode[0] = D12 / 10;
                tempCode[1] = D12 % 10;
            }
            else
            {
                Console.WriteLine("Please Select the right Category");
                return;
            }

            // Step 2: Get product type
            Console.Write("Enter the Type (Shirt, T-Shirt, Trouser, Short, Sports Kit, Skirt, Blouse, Saree): ");
            string selectedType = Console.ReadLine();

            if (Dtype.ContainsKey(selectedType))
            {
                int D34 = Dtype.Get(selectedType);
                tempCode[2] = D34 / 10;
                tempCode[3] = D34 % 10;
            }
            else
            {
                Console.WriteLine("Please Select the right Type");
                return;
            }

            // Step 3: Get supplier code
            Console.Write("Enter the 3-digit Supplier Code: ");
            if (int.TryParse(Console.ReadLine(), out int D567) && D567 >= 100 && D567 <= 999)
            {
                tempCode[4] = D567 / 100;
                tempCode[5] = (D567 % 100) / 10;
                tempCode[6] = D567 % 10;
            }
            else
            {
                Console.WriteLine("Invalid Supplier Code. It must be a 3-digit number.");
                return;
            }

            // Step 4: Generate sequential number
            string first7Digits = $"{tempCode[0]}{tempCode[1]}{tempCode[2]}{tempCode[3]}{tempCode[4]}{tempCode[5]}{tempCode[6]}";
            int categoryIndex = tempCode[0] * 10 + tempCode[1] - 1; // Convert to 0-based index

            // Find the next sequential number
            int sequentialNumber = 1;
            for (int i = 0; i < productArrays[categoryIndex].Count; i++)
            {
                object[] product = (object[])productArrays[categoryIndex][i];
                string existingCode = (string)product[0];
                if (existingCode.StartsWith(first7Digits))
                {
                    sequentialNumber++;
                }
            }

            // Add the sequential number to the code
            tempCode[7] = sequentialNumber / 100;
            tempCode[8] = (sequentialNumber % 100) / 10;
            tempCode[9] = sequentialNumber % 10;

            // Generate the final product code
            string productCode = $"{tempCode[0]}{tempCode[1]}{tempCode[2]}{tempCode[3]}{tempCode[4]}{tempCode[5]}{tempCode[6]}{tempCode[7]}{tempCode[8]}{tempCode[9]}";

            // Step 5: Get cost price, selling price, and quantity
            Console.Write("Enter Cost Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal costPrice))
            {
                Console.WriteLine("Invalid cost price!");
                return;
            }

            Console.Write("Enter Selling Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal sellingPrice))
            {
                Console.WriteLine("Invalid selling price!");
                return;
            }

            Console.Write("Enter Quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity))
            {
                Console.WriteLine("Invalid quantity!");
                return;
            }

            // Step 6: Create a 4-element array
            object[] productDetails = new object[4]
            {
        productCode,       // Index 0: Product Code
        costPrice,         // Index 1: Cost Price
        sellingPrice,      // Index 2: Selling Price
        quantity           // Index 3: Quantity
            };

            // Step 7: Save the product in the appropriate dynamic array
            productArrays[categoryIndex].Add(productDetails);

            // Step 8: Display the product code
            Console.WriteLine("Code of the Product: " + productCode);

            // Step 9: Menu options after adding a product
            Console.WriteLine("\n\t1. Add another Product");
            Console.WriteLine("\t2. Main Menu");
            Console.WriteLine("\t3. Exit");

            Console.Write("Enter Your Choice: ");
            if (int.TryParse(Console.ReadLine(), out int finish))
            {
                switch (finish)
                {
                    case 1:
                        AddProduct();
                        break;
                    case 2:
                        MStore();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid Choice.");
                        break;
                }
            }
        }
        private int GetCategoryIndex(string category)
        {
            if (Costomer.ContainsKey(category))
                return Costomer.Get(category) - 1; // Convert to 0-based index
            else
                throw new ArgumentException("Invalid category");
        }

        public void CategorizeProduct()
        {
            // Aggregate all products into a list
            List<object[]> allProducts = new List<object[]>();
            for (int i = 0; i < productArrays.Length; i++)
            {
                DynamicArray categoryArray = productArrays[i];
                for (int j = 0; j < categoryArray.Count; j++)
                {
                    object[] product = (object[])categoryArray[j];
                    allProducts.Add(product);
                }
            }

            if (allProducts.Count == 0)
            {
                Console.WriteLine("No products available.");
                return;
            }

            // Ask for sorting criteria
            Console.WriteLine("\nHow would you like to categorize the products?");
            Console.WriteLine("1. Category Wise");
            Console.WriteLine("2. In Order of Cost Price");
            Console.WriteLine("3. In Order of Selling Price");
            Console.Write("Enter Your Choice: ");

            if (!int.TryParse(Console.ReadLine(), out int choice) || choice < 1 || choice > 3)
            {
                Console.WriteLine("Invalid choice.");
                return;
            }

            // Define the comparison logic
            Func<object[], object[], int> compare = null;
            switch (choice)
            {
                case 1: // Category Wise (sort by first 2 digits of product code)
                    compare = (a, b) =>
                    {
                        string aCode = (string)a[0];
                        string bCode = (string)b[0];
                        int aCategory = int.Parse(aCode.Substring(0, 2));
                        int bCategory = int.Parse(bCode.Substring(0, 2));
                        return aCategory.CompareTo(bCategory);
                    };
                    break;

                case 2: // Cost Price
                    compare = (a, b) =>
                    {
                        decimal aCost = (decimal)a[1];
                        decimal bCost = (decimal)b[1];
                        return aCost.CompareTo(bCost);
                    };
                    break;

                case 3: // Selling Price
                    compare = (a, b) =>
                    {
                        decimal aSelling = (decimal)a[2];
                        decimal bSelling = (decimal)b[2];
                        return aSelling.CompareTo(bSelling);
                    };
                    break;
            }

            // Sort using merge sort
            List<object[]> sortedProducts = MergeSort(allProducts, compare);

            // Display sorted products
            Console.WriteLine("\n--- Sorted Products ---");
            foreach (object[] product in sortedProducts)
            {
                Console.WriteLine(
                    $"Code: {product[0]}, " +
                    $"Cost: {product[1]}, " +
                    $"Price: {product[2]}, " +
                    $"Qty: {product[3]}"
                );
            }
            Console.WriteLine("-----------------------");
        }

        public void EditProduct()
        {
            // Step 1: Ask for the product code to edit
            Console.Write("Enter the Product Code to Edit: ");
            string productCode = Console.ReadLine();

            // Step 2: Search for the product in all categories
            bool found = false;
            for (int i = 0; i < productArrays.Length; i++)
            {
                for (int j = 0; j < productArrays[i].Count; j++)
                {
                    object[] product = (object[])productArrays[i][j];
                    if ((string)product[0] == productCode)
                    {
                        found = true;

                        // Step 3: Display current details
                        Console.WriteLine("\nCurrent Product Details:");
                        Console.WriteLine($"Code: {product[0]}");
                        Console.WriteLine($"Cost Price: {product[1]}");
                        Console.WriteLine($"Selling Price: {product[2]}");
                        Console.WriteLine($"Quantity: {product[3]}");

                        // Step 4: Ask which detail to edit
                        Console.WriteLine("\nWhat would you like to edit?");
                        Console.WriteLine("1. Cost Price");
                        Console.WriteLine("2. Selling Price");
                        Console.WriteLine("3. Add Stock");
                        Console.WriteLine("4. Remove Stock");
                        Console.Write("Enter Your Choice: ");

                        if (int.TryParse(Console.ReadLine(), out int editChoice))
                        {
                            switch (editChoice)
                            {
                                case 1: // Edit Cost Price
                                    Console.Write("Enter New Cost Price: ");
                                    if (decimal.TryParse(Console.ReadLine(), out decimal newCostPrice))
                                    {
                                        product[1] = newCostPrice;
                                        Console.WriteLine("Cost Price updated successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid cost price. No changes made.");
                                    }
                                    break;

                                case 2: // Edit Selling Price
                                    Console.Write("Enter New Selling Price: ");
                                    if (decimal.TryParse(Console.ReadLine(), out decimal newSellingPrice))
                                    {
                                        product[2] = newSellingPrice;
                                        Console.WriteLine("Selling Price updated successfully!");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid selling price. No changes made.");
                                    }
                                    break;

                                case 3: // Add Stock
                                    Console.Write("Enter Quantity to Add: ");
                                    if (int.TryParse(Console.ReadLine(), out int addQuantity) && addQuantity > 0)
                                    {
                                        int currentQuantity = (int)product[3];
                                        product[3] = currentQuantity + addQuantity;
                                        Console.WriteLine($"Stock updated successfully! New Quantity: {product[3]}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid quantity. No changes made.");
                                    }
                                    break;

                                case 4: // Remove Stock
                                    Console.Write("Enter Quantity to Remove: ");
                                    if (int.TryParse(Console.ReadLine(), out int removeQuantity) && removeQuantity > 0)
                                    {
                                        int currentQuantity = (int)product[3];
                                        if (removeQuantity <= currentQuantity)
                                        {
                                            product[3] = currentQuantity - removeQuantity;
                                            Console.WriteLine($"Stock updated successfully! New Quantity: {product[3]}");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Cannot remove more than the current stock.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid quantity. No changes made.");
                                    }
                                    break;

                                default:
                                    Console.WriteLine("Invalid choice. No changes made.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. No changes made.");
                        }

                        // Step 5: Ask if the user wants to make another change
                        Console.WriteLine("\n1. Make Another Change");
                        Console.WriteLine("2. Main Menu");
                        Console.WriteLine("3. Exit");
                        Console.Write("Enter Your Choice: ");

                        if (int.TryParse(Console.ReadLine(), out int finish))
                        {
                            switch (finish)
                            {
                                case 1:
                                    EditProduct(); // Recursively call EditProduct
                                    break;
                                case 2:
                                    MStore(); // Return to the main menu
                                    break;
                                case 3:
                                    Environment.Exit(0); // Exit the program
                                    break;
                                default:
                                    Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                                    MStore();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                            MStore();
                        }

                        break;
                    }
                }
                if (found) break;
            }

            // Step 6: Handle case where product is not found
            if (!found)
            {
                Console.WriteLine("Product not found!");
            }
        }

        public void DeleteProduct()
        {
            // Step 1: Ask for the product code to delete
            Console.Write("Enter the Product Code to Delete: ");
            string productCode = Console.ReadLine();

            // Step 2: Search for the product in all categories
            bool found = false;
            for (int i = 0; i < productArrays.Length; i++)
            {
                for (int j = 0; j < productArrays[i].Count; j++)
                {
                    object[] product = (object[])productArrays[i][j];
                    if ((string)product[0] == productCode)
                    {
                        found = true;

                        // Step 3: Display product details
                        Console.WriteLine("\nProduct Details:");
                        Console.WriteLine($"Code: {product[0]}");
                        Console.WriteLine($"Cost Price: {product[1]}");
                        Console.WriteLine($"Selling Price: {product[2]}");
                        Console.WriteLine($"Quantity: {product[3]}");

                        // Step 4: Confirm deletion
                        Console.Write("\nAre you sure you want to delete this product? (yes/no): ");
                        string confirmation = Console.ReadLine().ToLower();

                        if (confirmation == "yes")
                        {
                            // Step 5: Remove the product from the dynamic array
                            productArrays[i].RemoveAt(j);
                            Console.WriteLine("Product deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Deletion canceled.");
                        }

                        // Step 6: Ask if the user wants to delete another product
                        Console.WriteLine("\n1. Delete Another Product");
                        Console.WriteLine("2. Main Menu");
                        Console.WriteLine("3. Exit");
                        Console.Write("Enter Your Choice: ");

                        if (int.TryParse(Console.ReadLine(), out int finish))
                        {
                            switch (finish)
                            {
                                case 1:
                                    DeleteProduct(); // Recursively call DeleteProduct
                                    break;
                                case 2:
                                    MStore(); // Return to the main menu
                                    break;
                                case 3:
                                    Environment.Exit(0); // Exit the program
                                    break;
                                default:
                                    Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                                    MStore();
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                            MStore();
                        }

                        break;
                    }
                }
                if (found) break;
            }

            // Step 7: Handle case where product is not found
            if (!found)
            {
                Console.WriteLine("Product not found!");

                // Step 8: Ask if the user wants to try again
                Console.WriteLine("\n1. Try Again");
                Console.WriteLine("2. Main Menu");
                Console.WriteLine("3. Exit");
                Console.Write("Enter Your Choice: ");

                if (int.TryParse(Console.ReadLine(), out int retryChoice))
                {
                    switch (retryChoice)
                    {
                        case 1:
                            DeleteProduct(); // Recursively call DeleteProduct
                            break;
                        case 2:
                            MStore(); // Return to the main menu
                            break;
                        case 3:
                            Environment.Exit(0); // Exit the program
                            break;
                        default:
                            Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                            MStore();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Choice. Returning to Main Menu.");
                    MStore();
                }
            }
        }
    }
}

  
        
    
