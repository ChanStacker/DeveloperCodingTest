# Developer Coding Test - Hacker News API

## Documentation

* Open the solution in VS 2022 and press F5 to launch the API
* Default url is currently set to `http://localhost:5038/best-stories/1`
* To retrieve top n stories retrieved, change the URL parameter from 1 to n

## Assumptions

* That the list of best stories has not been ordered by descending score already, therefore the sorting and selecting n stories has to be done by the Gateway
* That 100 requests per minute to the HackerNews Api is an acceptable max limit considering that the entire list of best stories has to be retrieved  from HackerNews API and deserialised on each call.

## Improvements

* Extract interactions with the HackerNews Api into HackerNewsRepository where HttpClientFactory will be injected.
* Add logging using Serilog
* Implement unit test on the sorting and selecting logic by mocking the HackerNewsRepository
* Make use of the Fakes utility in the unit test project and unit test different scenarios of response payload.