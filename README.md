# Umbraco-headless

## Demo project to demonstrate a headless approach to running Umbraco 10

This project relies on the Vertica Umbraco Headless Framework and showcases how it can be utilized to serve content from Umbraco as JSON output.
(https://github.com/vertica-as/Vertica.Umbraco.Headless)

The Vertica Umbraco Headless Framework ships with a collection of default property renderes, responsible for serializing content of 
a given property type to JSON. As developers, we however often face a need to add functionality to certain content types in Umbraco. 
Examples of this could include:

- fetching data from a 3rd party data provider (eg. Vimeo, Shopify, Google Maps)
- fetching related content from Umbraco based on tags or a content picker
- formatting data (converting bytes to mb for filesizes, date formatting etc.)

Examples demonstrated in this demo includes:

- Overriding the default rendering for a specific page type and adding data fetched from 3rd party (HomeHandler.cs)
- Overriding the default rendering of specific content blocks (BlockListHandler.cs & BlockListService.cs)
- Using Umbraco Modelsbuilder for generating strongly typed Models based on document and element types created in Umbraco
