import React, { useState, useEffect } from "react"
import { createAuthHeaders } from "../../API/userManager"

/*

    The context is imported and used by individual components

    that need data

*/

export const PlaylistContext = React.createContext()

/*

 This component establishes what data can be used.

 */

export const PlaylistProvider = (props) => {

    const [playlists, setPlaylists] = useState([])

    const getPlaylists = () => {
        const authHeader = createAuthHeaders();
        return fetch("https://localhost:5001/api/playlists", {

            headers: authHeader
        })

            .then(res => res.json())
            .then(setPlaylists)

    }

    const saveImages = async (files) => {
        const authHeader = createAuthHeaders();
        const formData = new FormData();
        if (files) {
            Array.from(files).forEach(file => {
                formData.append(file.name, file);
            });
        }

        const response = await fetch('https://localhost:5001/api/playlists/files', {

            // content-type header should not be specified!
            method: 'POST',
            headers: {
                authHeader,
            },
            body: formData,
            responseType: 'text'
        });
        return response.text();
    }

    const saveImage = async (file) => {
        const authHeader = createAuthHeaders();
        const formData = new FormData();
        if (file) {
            formData.append(file.name, file); 
        }

        const response = await fetch('https://localhost:5001/api/playlists/files', {

            // content-type header should not be specified!
            method: 'POST',
            headers: {
                authHeader,
            },
            body: formData,
            responseType: 'text'
        });
        return response.text();
    }

    const addPlaylist = playlist => {

        const authHeader = createAuthHeaders();

        var form_data = new FormData();

        console.log(playlist);

        for (var key in playlist) {
            form_data.append(key, playlist[key]);
        }

        for (var key of form_data.entries()) {
            console.log(key[0] + ', ' + key[1]);
        }

        return fetch("https://localhost:5001/api/playlists", {


            method: "POST",

            headers: {
                authHeader
            },

            body: form_data
        })

            .then(getPlaylists)

    }

    const deletePlaylist = playlist => {
        const authHeader = createAuthHeaders();
        return fetch(`https://localhost:5001/api/playlists/${playlist.id}`, {

            authHeader,
            method: "DELETE"

        })

            .then(getPlaylists)

    }

    const updatePlaylist = playlist => {

        const authHeader = createAuthHeaders();
        return fetch(`https://localhost:5001/api/playlists/${playlist.id}`, {

            method: "PUT",

            headers: {
                authHeader,
                "Content-Type": "application/json"

            },

            body: JSON.stringify(playlist)

        })

            .then(getPlaylists)

    }

    /*

        Load all playlists when the component is mounted. Ensure that
        an empty array is the second argument to avoid infinite loop.

    */

    useEffect(() => {

        getPlaylists()

    }, [])

    useEffect(() => {

        //console.log("****  CAR APPLICATION STATE CHANGED  ****")
        //console.log(playlists)

    }, [playlists])

    return (

        <PlaylistContext.Provider value={{

            playlists, addPlaylist, deletePlaylist, updatePlaylist, saveImages, saveImage

        }}>

            {props.children}

        </PlaylistContext.Provider>

    )

}