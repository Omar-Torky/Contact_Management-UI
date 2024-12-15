type Contact = {
    Name: string
    PhoneNumber: string
    Email: string
}

let mutable contacts = Map.empty<string, Contact>

let addContact name phoneNumber email =
    let newContact = { Name = name; PhoneNumber = phoneNumber; Email = email }
    contacts <- contacts.Add(phoneNumber, newContact)
    printfn "Contact added: %A" newContact

// let searchContact key =
//     let results = 
//         contacts 
//         |> Map.filter (fun _ contact -> contact.Name.Contains(key) || contact.PhoneNumber.Contains(key))
//     if results.IsEmpty then
//         printfn "No contacts found."
//     else
//         results |> Map.iter (fun _ contact -> printfn "%A" contact)


let editContact phoneNumber newName newPhone newEmail =
    match contacts.TryFind(phoneNumber) with
    | Some _ ->
        let updatedContact = { Name = newName; PhoneNumber = newPhone; Email = newEmail }
        contacts <- contacts.Remove(phoneNumber).Add(newPhone, updatedContact)
        printfn "Contact updated: %A" updatedContact
    | None -> printfn "Contact not found."


let deleteContact phoneNumber =
    if contacts.ContainsKey(phoneNumber) then
        contacts <- contacts.Remove(phoneNumber)
        printfn "Contact deleted."
    else
        printfn "Contact not found."


let rec menu () =
    printfn "\nContact Management System"
    printfn "1. Add Contact"
    printfn "2. Search Contact"
    printfn "3. Edit Contact"
    printfn "4. Delete Contact"
    printfn "5. Exit"
    printf "Choose an option: "
    let choice = System.Console.ReadLine()

    match choice with
    | "1" ->
        printf "Enter Name: "
        let name = System.Console.ReadLine()
        printf "Enter Phone Number: "
        let phone = System.Console.ReadLine()
        printf "Enter Email: "
        let email = System.Console.ReadLine()
        addContact name phone email
        menu ()
    | "2" ->
        printf "Enter Name or Phone Number to Search: "
        let key = System.Console.ReadLine()
        // searchContact key
        menu ()
    | "3" ->
        printf "Enter Phone Number of Contact to Edit: "
        let phone = System.Console.ReadLine()
        printf "Enter New Name: "
        let newName = System.Console.ReadLine()
        printf "Enter New Phone Number: "
        let newPhone = System.Console.ReadLine()
        printf "Enter New Email: "
        let newEmail = System.Console.ReadLine()
        editContact phone newName newPhone newEmail
        menu ()
    | "4" ->
        printf "Enter Phone Number of Contact to Delete: "
        let phone = System.Console.ReadLine()
        deleteContact phone
        menu ()
    | "5" -> printfn "Exiting..."
    | _ ->
        printfn "Invalid option. Try again."
        menu ()
