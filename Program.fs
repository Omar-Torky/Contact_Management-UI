open System
open System.IO
open System.Text.RegularExpressions

type Contact = {
    Name: string
    PhoneNumber: string
    Email: string
}

let mutable contacts = Map.empty<string, Contact>
let filePath = "contacts.txt"

let isValidPhoneNumber (phoneNumber: string) =
    Regex.IsMatch(phoneNumber, @"^\+?[0-9]{10,15}$")

let isValidEmail (email: string) =
    Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")

// Toqa - add function
let addContact name phoneNumber email =
    if not (isValidPhoneNumber phoneNumber) then
        printfn "Invalid phone number format."
    elif not (isValidEmail email) then
        printfn "Invalid email format."
    elif contacts.ContainsKey(phoneNumber) then
        printfn "A contact with this phone number already exists."
    else
        let newContact = { Name = name; PhoneNumber = phoneNumber; Email = email }
        contacts <- contacts.Add(phoneNumber, newContact)
        let contactLine = $"{name},{phoneNumber},{email}"
        File.AppendAllText(filePath, contactLine + Environment.NewLine)
        printfn "Contact added: %A" newContact

// Basma - Search function
let searchContact (key: string) =
    if File.Exists(filePath) then
        let results =
            File.ReadLines(filePath)
            |> Seq.filter (fun line -> 
                not (String.IsNullOrWhiteSpace(line)) && line.Contains(key)) // Explicitly specify 'key' is a string
            |> Seq.toList
        if results.IsEmpty then
            printfn "No contacts found."
        else
            results |> List.iter (printfn "Found: %s")
    else
        printfn "File not found."

// Rahma - Edit function
let editContact (phoneNumber: string) (newName: string) (newPhone: string) (newEmail: string) =
    if not (File.Exists(filePath)) then
        printfn "File not found."
    else
        let lines = File.ReadAllLines(filePath) |> Array.toList
        let contactExists = 
            lines
            |> List.exists (fun line -> line.Contains(phoneNumber))

        if not contactExists then
            printfn "Contact not found."
        elif not (isValidPhoneNumber newPhone) then
            printfn "Invalid phone number format."
        elif not (isValidEmail newEmail) then
            printfn "Invalid email format."
        else
            let updatedLines =
                lines
                |> List.map (fun line ->
                    if line.Contains(phoneNumber) then
                        let parts = line.Split(',')
                        sprintf "%s,%s,%s" newName newPhone newEmail
                    else
                        line)
            
            File.WriteAllLines(filePath, updatedLines)
            printfn "Contact updated: Name=%s, Phone=%s, Email=%s" newName newPhone newEmail


// Bassant - Delete function
let deleteContact phoneNumber =
    if contacts.ContainsKey(phoneNumber) then
        contacts <- contacts.Remove(phoneNumber)
        printfn "Contact deleted."
    else
        printfn "Contact not found."

        

let saveContactsToFile (filePath: string) =
    use writer = new StreamWriter(filePath, append = true)
    contacts |> Map.iter (fun _ contact ->
        writer.WriteLine($"{contact.Name}\t{contact.PhoneNumber}\t{contact.Email}\n")
    )
    printfn "Contacts saved to %s" filePath

// Youssef - load contacts function
let loadContactsFromFile (filePath: string) =
    if File.Exists(filePath) then
        try
            use reader = new StreamReader(filePath)
            let lines = reader.ReadToEnd().Split([| '\n' |], StringSplitOptions.RemoveEmptyEntries)
            for line in lines do
                let parts = line.Split(',')
                if parts.Length = 3 then
                    let name = parts.[0].Trim()
                    let phoneNumber = parts.[1].Trim()
                    let email = parts.[2].Trim()
                    addContact name phoneNumber email
                else
                    printfn "Skipping invalid line: %s" line
            printfn "Contacts loaded from %s" filePath
        with
        | :? System.Exception as ex ->
            printfn "Error loading contacts: %s" ex.Message
    else
        printfn "File not found: %s" filePath


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
    let btnAdd = new Button(Text = "Add Contact", Top = 140, Left = 150, Width = 500, Height = 40)
    btnAdd.Click.Add(fun _ ->
        let name = InputBox("Enter Name:", "Add Contact")
        let phone = InputBox("Enter Phone:", "Add Contact")
        let email = InputBox("Enter Email:", "Add Contact")
        
        if String.IsNullOrWhiteSpace(name) || String.IsNullOrWhiteSpace(phone) || String.IsNullOrWhiteSpace(email) then
            MessageBox.Show("All fields are required.", "Error") |> ignore
        elif not (isValidPhoneNumber phone) then
            MessageBox.Show("Invalid phone number format.", "Error") |> ignore
        elif not (isValidEmail email) then
            MessageBox.Show("Invalid email format.", "Error") |> ignore
        elif contacts.ContainsKey(phone) then
            MessageBox.Show("A contact with this phone number already exists.", "Error") |> ignore
        else
            addContact name phone email
            MessageBox.Show("Contact added successfully.", "Success") |> ignore
    )



// Rahma - Edit Button
let btnEdit = new Button(Text = "Edit Contact", Top = 220, Left = 150, Width = 500, Height = 40)
btnEdit.Click.Add(fun _ ->
    let phoneToEdit = InputBox("Enter Phone Number of the Contact to Edit:", "Edit Contact")

    if not (String.IsNullOrWhiteSpace phoneToEdit) then
        let newName = InputBox("Enter New Name:", "Edit Contact")
        let newPhone = InputBox("Enter New Phone:", "Edit Contact")
        let newEmail = InputBox("Enter New Email:", "Edit Contact")

        // Check if new fields are valid before editing the contact
        if String.IsNullOrWhiteSpace(newName) || String.IsNullOrWhiteSpace(newPhone) || String.IsNullOrWhiteSpace(newEmail) then
            MessageBox.Show("All fields are required.", "Error") |> ignore
        elif not (isValidPhoneNumber newPhone) then
            MessageBox.Show("Invalid phone number format.", "Error") |> ignore
        elif not (isValidEmail newEmail) then
            MessageBox.Show("Invalid email format.", "Error") |> ignore
        else
            // If all fields are valid, proceed to edit the contact
            editContact phoneToEdit newName newPhone newEmail
            MessageBox.Show("Contact updated successfully.", "Success") |> ignore
    else
        MessageBox.Show("Phone number cannot be empty.", "Error") |> ignore
)
    



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
let btnDelete = new Button(Text = "Delete Contact", Top = 380, Left = 150, Width = 500, Height = 40)
    btnDelete.Click.Add(fun _ ->
        let phone = InputBox("Enter The Number You Wanna Delete:", "Delete Contact")
        
        if String.IsNullOrWhiteSpace(phone) then
            MessageBox.Show("Phone number cannot be empty.", "Error") |> ignore
        elif not (isValidPhoneNumber phone) then
            MessageBox.Show("Invalid phone number.", "Error") |> ignore
        else
            if contacts.ContainsKey(phone) then
                deleteContact phone
                MessageBox.Show($"Contact with phone number {phone} deleted successfully.", "Delete Contact") |> ignore
            else
                MessageBox.Show($"No contact found with phone number {phone}.", "Error") |> ignore
    )






// Youssef - load contacts Button
let btnLoadContacts = new Button(Text = "View Contacts", Top = 460, Left = 150, Width = 500, Height = 40)
    btnLoadContacts.Click.Add(fun _ ->
        try
            let filePath = File.ReadAllText("File.txt")

            // loadContactsFromFile filePath

            MessageBox.Show($"{filePath}") |> ignore
        with
        | :? FileNotFoundException ->
            MessageBox.Show("File not found! Please ensure the file path is correct.", "Error") |> ignore
        | :? System.Exception as ex ->
            MessageBox.Show($"An error occurred while loading the contacts: {ex.Message}", "Error") |> ignore
    )




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

