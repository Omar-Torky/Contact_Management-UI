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
let deleteContact (phoneNumber: string) =
    if File.Exists(filePath) then
        let lines = File.ReadAllLines(filePath) |> Array.toList
        let updatedLines =
            lines
            |> List.filter (fun line ->
                not (String.IsNullOrWhiteSpace(line)) && not (line.Contains(phoneNumber))) // Explicitly specify 'phoneNumber' as string
        if lines.Length = updatedLines.Length then
            printfn "No contact found with phone number: %s" phoneNumber
        else
            File.WriteAllLines(filePath, updatedLines)
            printfn "Contact with phone number %s deleted successfully." phoneNumber
    else
        printfn "File not found."
 


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
    




    //Add contact BTN
    //-------------------

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


    //Edit contact BTN
    //-------------------

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
    


    //Search contact BTN
    //---------------------
    let btnSearch = new Button(Text = "Search Contact", Top = 300, Left = 150, Width = 500, Height = 40)
    btnSearch.Click.Add(fun _ ->
        let key = InputBox("Enter Name or Phone Number to Search:", "Search Contact")
        
        if not (String.IsNullOrWhiteSpace key) then
            try
                if File.Exists(filePath) then
                    let fileContent = File.ReadAllLines(filePath)
                    let results =
                        fileContent
                        |> Array.filter (fun line -> 
                            line.Contains(key, StringComparison.OrdinalIgnoreCase)) // Case-insensitive search

                    if results.Length = 0 then
                        MessageBox.Show("No contacts found.", "Search Result") |> ignore
                    else
                        let resultMessage = 
                            results |> Array.fold (fun acc line -> acc + line + "\n") ""

                        MessageBox.Show($"Found contacts:\n{resultMessage}", "Search Result") |> ignore
                else
                    MessageBox.Show("File not found. Please save some contacts first.", "Error") |> ignore
            with
            | :? IOException as ex ->
                MessageBox.Show($"An error occurred while reading the file: {ex.Message}", "Error") |> ignore
            | ex ->
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error") |> ignore
        else
            MessageBox.Show("Please enter a valid search query.", "Error") |> ignore
    )


    //Delete contact BTN
    //---------------------
    let btnDelete = new Button(Text = "Delete Contact", Top = 380, Left = 150, Width = 500, Height = 40)
    btnDelete.Click.Add(fun _ ->
        let phone = InputBox("Enter The Number You Wanna Delete:", "Delete Contact")
        
        if String.IsNullOrWhiteSpace(phone) then
            MessageBox.Show("Phone number cannot be empty.", "Error") |> ignore
        elif not (isValidPhoneNumber phone) then
            MessageBox.Show("Invalid phone number.", "Error") |> ignore
         else
        try
            deleteContact phone
            MessageBox.Show($"Operation completed. Check console for details.", "Delete Contact") |> ignore
        with
        | :? IOException as ex ->
            MessageBox.Show($"An error occurred while accessing the file: {ex.Message}", "Error") |> ignore
        | ex ->
            MessageBox.Show($"Unexpected error: {ex.Message}", "Error") |> ignore
    )




    //Load contact BTN
    //-------------------
    let btnLoadContacts = new Button(Text = "View Contacts", Top = 460, Left = 150, Width = 500, Height = 40)

    btnLoadContacts.Click.Add(fun _ ->
        try
            let fileContents = File.ReadAllText(filePath)

            MessageBox.Show(fileContents, "Contacts") |> ignore
        with
        | :? FileNotFoundException ->
            MessageBox.Show("File not found! Please ensure the file path is correct.", "Error") |> ignore
        | :? System.Exception as ex ->
            MessageBox.Show($"An error occurred while loading the contacts: {ex.Message}", "Error") |> ignore

    )




    

    form.Controls.Add(btnAdd)
    form.Controls.Add(btnSearch)
    form.Controls.Add(btnDelete)
    form.Controls.Add(btnEdit)
    form.Controls.Add(btnLoadContacts)

    form




[<EntryPoint>]
let main argv =
    let form = createForm()
    Application.Run(form)
    0


