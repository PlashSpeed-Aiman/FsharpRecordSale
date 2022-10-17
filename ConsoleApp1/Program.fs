open System.Data.SQLite


// For more information see https://aka.ms/fsharp-console-apps
module App = 
    open System
    type Item ={
        ItemName : string
        Price    : float
        Quantity : int32
    }



    let RecordSale () = 
        Console.WriteLine "Item"
        let item = Console.ReadLine ()
        Console.WriteLine "Price"
        let price = Console.ReadLine ()
        Console.WriteLine "Quantity"
        let quantity = Console.ReadLine ()
        let SaleItem = {ItemName = item; Price = float(price); Quantity = int32(quantity)}
        SaleItem
    
    let OpenDBConnetion() =
        let sqlite_conn = new SQLiteConnection("Data Source=db.db;Version=3;New=True;Compress=True;")
        sqlite_conn.Open()
        sqlite_conn
    let SendToDatabase (item:Item) (x:SQLiteConnection) :unit  = 
        
        
        ()

    [<EntryPoint>]
    let main argv =
        let conn = OpenDBConnetion()
        let mutable app_condition = true
        while app_condition do
            RecordSale() |> fun x -> SendToDatabase x conn
            Console.WriteLine "Yes Or No?"
            let app_condition_str = Console.ReadLine()
            match app_condition_str with
                |"Y"|"y" -> app_condition <- false
                |_ -> app_condition <- true

        Console.WriteLine "Thank You for Using RecordIt!"
        Console.ReadKey |> ignore
        0


    