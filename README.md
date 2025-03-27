# Textile Management System

A C# console application for managing textile products in a store, featuring custom data structures and efficient product management.

## Features

- **Custom Data Structures**:
  - `CustomDictionary`: A hash-based dictionary implementation with chaining
  - `DynamicArray`: A resizable array implementation
- **Product Management**:
  - Add new products with auto-generated codes
  - Edit existing product details (price, quantity)
  - Delete products
  - Categorize products using merge sort algorithm
- **Product Categorization**:
  - Category-wise (Gents, Ladies, Boys, Girls, Kids)
  - Type-wise (Shirt, T-Shirt, Trouser, etc.)
  - Price-based sorting (cost price or selling price)

## Data Structures

### 1. CustomDictionary<K, V>
A custom hash table implementation featuring:
- Chaining collision resolution
- Dynamic resizing based on load factor
- Basic dictionary operations (Add, Get, ContainsKey)

### 2. DynamicArray
A resizable array implementation with:
- Automatic capacity expansion
- Basic operations (Add, RemoveAt, Indexer)

## Product Code Structure
Each product has a 10-digit code representing:
1. First 2 digits: Customer category (Gents, Ladies, etc.)
2. Next 2 digits: Product type (Shirt, T-Shirt, etc.)
3. Next 3 digits: Supplier code
4. Last 3 digits: Sequential number

## Sorting Algorithm
The system uses **merge sort** to efficiently categorize products by:
1. Category
2. Cost price
3. Selling price

## How to Use
1. Run the application
2. Select from the main menu:
   - Add new products
   - Edit existing products
   - Delete products
   - Categorize products
3. Follow the on-screen prompts

## Requirements
- .NET Framework or .NET Core
- C# compiler

## Future Improvements
- Add persistence (save/load data to/from files)
- Implement additional sorting options
- Add search functionality
- Improve error handling

## Author
Monishan Sangaralingam





