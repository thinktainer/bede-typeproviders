#r @"../../packages/FSharp.Data.2.0.9/lib/net40/FSharp.Data.dll"
#r @"../../packages/ApiaryProvider.1.0.2/lib/net40/ApiaryProvider.dll"

#load "Keys.fs"
open Keys

open ApiaryProvider
open FSharp.Data

let mdb = new ApiaryProvider<"themoviedb">("http://api.themoviedb.org")
mdb.AddQueryParam("api_key", ApiKey)

let query = "query", "craig"
let res = mdb.Search.Person(query=[query])

let totalPages = res.TotalPages

#time
let pages = 
    seq { 1 .. totalPages }
    |> Seq.map (fun x -> mdb.Search.AsyncPerson(query=[query; "page", x.ToString()]))
    |> Async.Parallel
    |> Async.RunSynchronously

let results = pages |> Array.collect(fun rs -> rs.Results)

let fi = res.Results |> Seq.filter(fun p -> p.Name.Contains("Daniel"))

let dan = fi |> Seq.exactlyOne



type dataprovider = FreebaseDataProvider<Key = GApiKey>
let fb = dataprovider.GetDataContext()
fb.Sports.Soccer.``Football world cups``.Individuals.``2014 FIFA World Cup``.Blurb
