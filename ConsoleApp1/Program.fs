open System.Data.SQLite


// For more information see https://aka.ms/fsharp-console-apps
module App = 
    open System
    type Item ={
        ItemName : string
        Price    : float
        Quantity : int64
    }
    
        
    let GetSales conn = 
       
        let reader = "SELECT * FROM ITEM" |> fun str -> new SQLiteCommand(str,conn) |> fun cmd -> cmd.ExecuteReader()
        let emptyList = []
        let rec test (reader:SQLiteDataReader)(rowList:Item list)  = 
            match reader.Read() with
                |false -> rowList
                |true  -> {ItemName=reader.["NAME"].ToString();Price = reader.["PRICE"]:?>float ;Quantity = reader.["QUANTITY"]:?>int64}::rowList |> test reader
        let itemList = test reader emptyList
        itemList
       

    let RecordSale () = 
        Console.WriteLine "Item"
        let item = Console.ReadLine ()
        Console.WriteLine "Price"
        let price = Console.ReadLine ()
        Console.WriteLine "Quantity"
        let quantity = Console.ReadLine ()
        let SaleItem = {ItemName = item; Price = float(price); Quantity = int64 (quantity)}
        SaleItem
    
    let OpenDBConnetion() =
        let sqlite_conn = new SQLiteConnection("Data Source=db.db;Version=3;New=True;Compress=True;")
        sqlite_conn.Open()
        sqlite_conn

    let SendToDatabase (item:Item) (x:SQLiteConnection) :unit  = 
        
       
        let insertSql = 
            $"INSERT INTO ITEM(NAME, PRICE, QUANTITY) " + 
            $"""values ("{item.ItemName}",{item.Price},{item.Quantity})"""
        let reader =  new SQLiteCommand(insertSql,x) |> fun cmd -> cmd.ExecuteNonQuery()
         
        ()

    [<EntryPoint>]
    let main argv =
        let conn = OpenDBConnetion()
        let mutable app_condition = true
        while app_condition do
            RecordSale() |> fun x -> SendToDatabase x conn
            GetSales conn |>  List.map Console.WriteLine |> ignore 
            
            Console.WriteLine "Exit Programme: Yes Or No?"
            let app_condition_str = Console.ReadLine()
            match app_condition_str with
                |"Y"|"y"|"Yes"|"YES" -> app_condition <- false
                |_ -> app_condition <- true
        conn.Close () |> ignore
        Console.WriteLine "Thank You for Using RecordIt!"
        Console.ReadKey |> ignore
        0


    