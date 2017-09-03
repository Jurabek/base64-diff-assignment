```
The following endpoints are available:

GET /                          - Usage instructions (this page)
GET /status                    - Service status
PUT /v1/diff/:id/left  {data}  - Set the left part of a diff
PUT /v1/diff/:id/right {data}  - Set the right part of a diff
GET /v1/diff/:id               - Shows the diff results



Examples
========


1. Checking the service status
-----------------------------------------
GET /status HTTP/1.0

HTTP/1.1 200 OK
Connection: close
Content-Type: text/plain; charset=utf-8

OK
-----------------------------------------


2. Setting the left part of a diff to the string "The quick brown fox jumps over the lazy dog"
----------------------------------------
PUT /v1/diff/1/left HTTP/1.0
Content-type: application/json

{
    "data": "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw=="
}

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"success":true}
-----------------------------------------


3. Checking the diff result
----------------------------------------
GET /v1/diff/1
Host: localhost

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"diff":"different-lengths","differences":[]}
-----------------------------------------


4. Setting the right part of the diff to the same string
----------------------------------------
PUT /v1/diff/1/right HTTP/1.0
Content-type: application/json

{
    "data": "VGhlIHF1aWNrIGJyb3duIGZveCBqdW1wcyBvdmVyIHRoZSBsYXp5IGRvZw=="
}

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"success":true}
-----------------------------------------


5. Checking the diff result
----------------------------------------
GET /v1/diff/1
Host: localhost

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"diff":"same-content","differences":[]}
-----------------------------------------


6. Setting the right part of the diff to a different string ("The lazy brown fox jumps over the quick dog")
----------------------------------------
PUT /v1/diff/1/right HTTP/1.0
Content-type: application/json

{
    "data": "VGhlIGxhenkgYnJvd24gZm94IGp1bXBzIG92ZXIgdGhlIHF1aWNrIGRvZw=="
}

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"success":true}
-----------------------------------------


7. Checking the diff result
----------------------------------------
GET /v1/diff/1
Host: localhost

HTTP/1.1 200 OK
Connection: close
Content-Type: application/json; charset=utf-8

{"diff":"different-content","differences":[{"offset":4,"length":35}]}
-----------------------------------------


8. And what if I send invalid data?
----------------------------------------
PUT /v1/diff/1/right HTTP/1.0
Content-type: application/json

{"data":"this_is_not_a_base64_string!"}

HTTP/1.1 422 Unprocessable Entity
Connection: close
Content-Type: application/json; charset=utf-8

{"error":"Malformed Base64 string data"}
-----------------------------------------
```
