	 
	 
name = "car"
-- whitelists for profile and meta
profile_whitelist = {
    "highway"
}
meta_whitelist = {
    "name"
}
-- profile definitions linking a function to a profile
profiles = {
    {
        name = "",
        function_name = "factor_and_speed",
        metric = "time"
    },
    { 
        name = "shortest",
        function_name = "factor_and_speed",
        metric = "distance",
    }
}
-- the main function turning attributes into a factor_and_speed and a tag whitelist
function factor_and_speed (attributes, result)

     result.speed = 0
     result.direction = 0
     result.canstop = true
     result.attributes_to_keep = {}

     -- get default speed profiles
     local highway = attributes.highway
     if highway == "motorway" or 
        highway == "motorway_link" then
        result.speed = 100 -- speed in km/h
        result.direction = 0
        result.canstop = true
        result.attributes_to_keep.highway = highway
    end
end