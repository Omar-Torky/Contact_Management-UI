open System
open System.IO
open System.Text.RegularExpressions

type Contact = {
    Name: string
    PhoneNumber: string
    Email: string
}

let mutable contacts = Map.empty<string, Contact>

let isValidPhoneNumber (phoneNumber: string) =
    Regex.IsMatch(phoneNumber, @"^\+?[0-9]{10,15}$")

let isValidEmail (email: string) =
    Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")

// Toqa - add function

// Basma - Search function
let searchContact key =
    let results = 
        contacts 
        |> Map.filter (fun _ contact -> contact.Name.Contains(key : string) || contact.PhoneNumber.Contains(key : string))
    if results.IsEmpty then
        printfn "No contacts found."
    else
        results |> Map.iter (fun _ contact -> printfn "%A" contact)

// Rahma - Edit function




// Bassant - Delete function

let saveContactsToFile (filePath: string) =
    use writer = new StreamWriter(filePath, append = true)
    contacts |> Map.iter (fun _ contact ->
        writer.WriteLine($"{contact.Name}\t{contact.PhoneNumber}\t{contact.Email}\n")
    )
    printfn "Contacts saved to %s" filePath

// Youssef - load contacts function


open System
open System.Windows.Forms

let InputBox(prompt, title) =
    let inputForm = new Form(Text = title, Width = 300, Height = 150)
    let lblPrompt = new Label(Text = prompt, Top = 10, Left = 10, Width = 250)
    let txtInput = new TextBox(Top = 40, Left = 10, Width = 250)
    let btnOk = new Button(Text = "OK", Top = 80, Left = 10)
    btnOk.DialogResult <- DialogResult.OK
    inputForm.Controls.AddRange([| lblPrompt; txtInput; btnOk |])
    inputForm.AcceptButton <- btnOk
    if inputForm.ShowDialog() = DialogResult.OK then txtInput.Text else "Fuck"
let createForm () =
    let form = new Form(Text = "Contact Management System", Width = 800, Height = 700)
    



// Toqa - add Button



// Rahma - Edit Button




// Basma - Search Button
let btnSearch = new Button(Text = "Search Contact", Top = 300, Left = 150, Width = 500, Height = 40)
    btnSearch.Click.Add(fun _ ->
        let key = InputBox("Enter Name or Phone Number to Search:", "Search Contact")
        
        if not (String.IsNullOrWhiteSpace key) then
            let results = 
                contacts
                |> Map.filter (fun _ contact -> 
                    contact.Name.Contains(key) || contact.PhoneNumber.Contains(key))

            if results.IsEmpty then
                MessageBox.Show("No contacts found.", "Search Result") |> ignore
            else
                let contactList = 
                    results
                    |> Map.fold (fun acc _ contact -> acc + $"{contact.Name} - {contact.PhoneNumber}\n") ""

                MessageBox.Show($"Found contacts:\n{contactList}", "Search Result") |> ignore
        else
            MessageBox.Show("Please enter a valid search query.", "Error") |> ignore
    )




// Bassant - Delete Button





// Youssef - load contacts Button





    let btnSaveContacts = new Button(Text = "Save Contacts", Top = 540, Left = 150, Width = 500, Height = 40)
    btnSaveContacts.Click.Add(fun _ ->
        let filePath = InputBox("Enter The File Path to Save Contacts:", "Save Contacts")
        if not (String.IsNullOrWhiteSpace filePath) then
            saveContactsToFile filePath
            MessageBox.Show($"Contacts Saved to {filePath} .", "Save Contacts") |> ignore

        else
            MessageBox.Show("Please provide a valid file path.", "Error") |> ignore
    )




    

    form.Controls.Add(btnAdd)
    form.Controls.Add(btnSearch)
    form.Controls.Add(btnDelete)
    form.Controls.Add(btnEdit)
    form.Controls.Add(btnLoadContacts)
    form.Controls.Add(btnSaveContacts)

    form




[<EntryPoint>]
let main argv =
    let form = createForm()
    Application.Run(form)
    0









// let rec menu () =
//     printfn "\nContact Management System"
//     printfn "1. Add Contact"
//     printfn "2. Search Contact"
//     printfn "3. Edit Contact"
//     printfn "4. Delete Contact"
//     printfn "5. Save Contacts to File"
//     printfn "6. Load Contacts from File"
//     printfn "7. Exit"
//     printf "Choose an option: "
//     let choice = System.Console.ReadLine()

//     match choice with
//     | "1" ->
//         printf "Enter Name: "
//         let name = System.Console.ReadLine()
//         printf "Enter Phone Number: "
//         let phone = System.Console.ReadLine()
//         printf "Enter Email: "
//         let email = System.Console.ReadLine()
//         addContact name phone email
//         menu ()
//     | "2" ->
//         printf "Enter Name or Phone Number to Search: "
//         let key = System.Console.ReadLine()
//         searchContact key
//         menu ()
//     | "3" ->
//         printf "Enter Phone Number of Contact to Edit"
//         let phoneNumber = System.Console.ReadLine()
//         printf "Enter New Name: "
//         let newName = System.Console.ReadLine()
//         printf "Enter New Phone Number: "
//         let newPhone = System.Console.ReadLine()
//         printf "Enter New Email: "
//         let newEmail = System.Console.ReadLine()
//         editContact phoneNumber newName newPhone newEmail
//         menu ()
//     | "4" ->
//         printf "Enter Phone Number of Contact to Delete: "
//         let phoneNumber = System.Console.ReadLine()
//         deleteContact phoneNumber
//         menu ()
//     | "5" ->
//         printf "Enter File Path to Save Contacts: "
//         let filePath = System.Console.ReadLine()
//         saveContactsToFile filePath
//         menu ()
//     | "6" ->
//         printf "Enter File Path to Load Contacts: "
//         let filePath = System.Console.ReadLine()
//         loadContactsFromFile filePath
//         menu ()
//     | "7" ->
//         printfn "Exiting the program. Goodbye!"
//     | _ ->
//         printfn "Invalid option. Please try again."
//         menu ()

// menu ()

