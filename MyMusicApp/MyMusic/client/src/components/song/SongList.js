import React, { useContext } from "react"
import { SongContext } from "./SongProvider"
import Song from "./Song"
import "./Songs.css"

export function SongList(props) {
    const { songs } = useContext(SongContext)
console.log("here")
    return (
        <div className="tabPanel">
            <h1>Songs</h1>

   <button onClick={() => props.history.push("/songs/create")}>
                Add Song
            </button>
            <div className="songs top-space">
                {
                    songs.map(song => {
                        return <Song key={song.id} song={song} />
                    })
                }
            </div>
        </div>
    )
}