# Simple Email Service

## Test scenarios

To add a contact via the post endpoint, use the following values in the body of the request:
```
{
  "name": "Weird Al",
  "birthDate": "1959-10-23",
  "emails": [
    {
      "isPrimary": true,
      "address": "weirdalemail@example.com"
    }
  ]
}
```

Updated value

```
{
  "id": 1,
  "name": "Weird Al",
  "birthDate": "1959-10-23",
  "emails": [
    {
      "isPrimary": true,
      "address": "weirdalemail@example.com"
    }
  ]
}
```