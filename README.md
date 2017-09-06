# C# SDK for SCG Messaging APIs

This is the C# version of the SCG API. 
The SCG APIs provides access to communication channels using SMS, MMS, 
Push Notification, OTT messaging and Voice. 

We have prepared a C# wrapper over the REST API for
these services. The C# API use the asynchronous features
introduced in C# 5 for maximum performance and code clearness.

The SDK require a .NET Standard 1.4 environment.

The C# SDK hides some of the REST API's constraints, like
lists being returned in logical pages of _n_ records. With the
C# SDK, the list method returns a generator that works with *foreach()*.

Please register for a free account at https://developer.syniverse.com to get your API keys.

## External dependencies
- [Newtonsoft.json](http://www.newtonsoft.com/json) ([nuget](https://www.nuget.org/packages/Newtonsoft.Json/))
- [System.Reactive](https://github.com/Reactive-Extensions/Rx.NET) ([nuget](https://www.nuget.org/packages/System.Reactive/))


## How to use the SDK
The C# SDK implements wrapper classes over the 
different Messaging API resources. Using these resource
classes, you can create, get, update, list, replace and delete
objects. Objects are mapped transparently between native C# classes
(in the SDK) and json (when communicating with the REST API). In 
short, you use C# classes in your code.

# Some examples

## List all contacts
```C#
static void ListContacts(SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = Contact.Resource(session);

        foreach (var contact in res.List())
        {
            Console.WriteLine("Contact id {0},  mdn {1}",
                contact.Id, contact.PrimaryMdn);
        }
    }
}
```
This program will output something like:
```
Contact id 4AOXpg8dlXRCMXlWDUch73  mdn: 155560000002
Contact id vsXgkqhUW4eIwMColyevn7  mdn: 155589823057
```
[Full example](Examples/list_contacts/Program.cs)


## Create a contact

You can create an object by calling Create() with an instance
of the type you want to create initialized with the 
properties you require.

```C#
 static async Task AddContact(String firstName, String phone, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var contactRes = Contact.Resource(session);
        String id = await contactRes.Create(new Contact()
        {
            FirstName = firstName,
            PrimaryMdn = phone
        });

        Console.WriteLine("Added contact {0} with name {1}",
            id, firstName, firstName);
    }
}

```
[Full example](Examples/add_contact/Program.cs)

In this example, we just set the first name and the phone number.

The example will output some thing like:
```
Added contact 9wAy5ydl6dxg3mqjLLvHM with name Alice
```

## Update a contact

In order to update an object, you first get an instance of the 
object using Get(). Then you change the desired properties, and
call Update().

```c#
static async Task UpdateContact(String id, String newName, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = Contact.Resource(session);

        var contact = await res.Get(id);

        string oldName = contact.FirstName;
        contact.FirstName = newName;

        await res.Update(contact.Id, contact);

        Console.WriteLine("Updated the name for contact {0} from {1} to {2}",
            contact.Id, oldName, contact.FirstName);
    }
}
```
[Full example](Examples/update_contact/Program.cs)

## Delete a contact
```C#
static async Task DeleteContact(String phoneNumber, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = Contact.Resource(session);

        // We have to use List() to search for the phone number. 
        // There should be at max one iteration of the loop.
        foreach (var contact in res.List(new Dictionary<String, String>()
            {
                {"primary_mdn", phoneNumber }
            }))
        {
            Console.WriteLine("Deleting contact {0} with mdn {1}",
                contact.Id, contact.PrimaryMdn);
            await res.Delete(contact.Id);
        }
    }
}
```
[Full example](Examples/delete_contact/Program.cs)

## Error handling
Errors are reported trough exceptions. Please see the 
C# [Newtonsoft.json](http://www.newtonsoft.com/json) library
for reference. The SDK will throw a ScgApi.ExceptionRequestFailed
exception if a request failed with a HTTP error response code.

# Some more examples

## Sending a SMS to a GSM number

```C#
static async Task SendSms(String senderId, String receipient, String body, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = MessageRequest.Resource(session);
        String id = await res.Create(new MessageRequest()
        {
            From = "sender_id:" + senderId,
            To = new List<String>() { receipient },
            Body = body
        });

        Console.WriteLine("Created message request {0}", id);
    }
}
```

[Full example](Examples/send_sms/Program.cs)

## Sending a Message to a Contact

This works as above, except for the *to* field in *create*.
```C#
String id = await res.Create(new MessageRequest()
    {
        From = "sender_id:" + senderId,
        To = new List<String>() { "contact:" + receipient },
        Body = body
    });

```

## Sending a Message to a Group

Here we will create two new contacts, a new group, assign the contacts
to the group, and then send a message to the group.

```C#
static async Task SendSmsToGroup(String senderId, String aliceMdn, 
            String bobMdn, String body, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        // Create a group
        var grpRes = ContactGroup.Resource(session);
        String friendsId = await grpRes.Create(new ContactGroup()
        {
            Name = "Friends"
        });

        // Create two contacts
        var contactRes = Contact.Resource(session);
        String aliceId = await contactRes.Create(new Contact()
        {
            FirstName = "Alice",
            PrimaryMdn = aliceMdn
        });
        String bobId = await contactRes.Create(new Contact()
        {
            FirstName = "Bob",
            PrimaryMdn = bobMdn
        });

        // Add the contacts to the group
        await grpRes.AddContact(friendsId, aliceId);
        await grpRes.AddContact(friendsId, bobId);

        // Send the message request
        var resMrq = MessageRequest.Resource(session);
        String mrqId = await resMrq.Create(new MessageRequest()
        {
            From = "sender_id:" + senderId,
            To = new List<String>() { "group:" + friendsId },
            Body = body
        });

        Console.WriteLine("Created message request {0} to group {1}", 
            mrqId, friendsId);
    }
}
```
[Full example](Examples/send_sms_to_grp/Program.cs)

## Sending a MMS with an attachment

```C#
static async Task<String> CreateAndUploadAttachment(Session session, String path)
{
    var res = Attachment.Resource(session);
    var id = await res.Create(new Attachment()
    {
        Name = "test_upload",
        Filename = "cutecat.jpg",
        Type = "image/jpeg"
    });

    await res.Upload(id, path);

    Console.WriteLine("Created attachment {0}", id);
    return id;
}

static async Task SendMms(String senderId, String receipient, String body, 
            String attachmentPath, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = MessageRequest.Resource(session);
        String id = await res.Create(new MessageRequest()
        {
            From = "sender_id:" + senderId,
            To = new List<String>() { receipient },
            Body = body,
            Attachments = new List<string>()
            {
                await CreateAndUploadAttachment(session, attachmentPath)
            }
        });

        Console.WriteLine("Created message request {0}", id);
    }
}
```

[Full example](Examples/send_mms/Program.cs)

## Checking the state of a Message Request

```C#
static async Task CheckState(String messageRequestId, SessionOptions opts)
{
    using (var session = new Session(opts))
    {
        var res = MessageRequest.Resource(session);

        var mrq = await res.Get(messageRequestId);

        Console.WriteLine("Message request {0} has state {1}",
            mrq.Id, mrq.State);

        foreach (var msg in res.ListMessages(mrq.Id))
        {
            Console.WriteLine("  Message {0} has state {1}",
                msg.Id, msg.State);
        }
    }
}
```

The example below may output something like:
```
Message request XW4NtdPXDwnOKqWuhKYHL3 has state COMPLETED
  Message IyOOfnAURiXVXbjrjeh9F1 has state SENT
```

[Full example](Examples/check_message_request_state/Program.cs)

