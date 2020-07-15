import React, { useContext } from "react"
import { UserContext } from "../user/UserProvider"
import { SongContext } from "./SongProvider"
import "./Songs.css"
import { Link } from "react-router-dom";
import { render } from "react-dom";

export default (props) => {

    const { songs, deleteSong } = useContext(SongContext)
    const { users } = useContext(UserContext)
    const chosenSongId = parseInt(props.match.params.songId, 10)
    const song = songs.find(s => s.id === chosenSongId) || {}
    const user = users.find(u => u.id === song.userId) || {}


    const renderDeleteButton = () => {

        if (song.userId !== parseInt(localStorage.getItem("currentUser"))) {
            return null;
        } else {
            return (

                <button className="deleteSongButton" onClick={
                    () => {
                        deleteSong(song)
                            .then(() => {
                                props.history.push("/")
                            })
                    }
                }>
                    Delete Song
            </button>
            );
        }
    }

    const renderEditButton = () => {

        if (song.userId !== parseInt(localStorage.getItem("currentUser"))) {
            return null;
        } else {
            return (

                <button className="editSongButton" onClick={() => {
                    props.history.push(`/songs/edit/${song.id}`)
                }}>Edit Song</button>

            );
        }
    }

    return (
        <section className="song">


            <img class="detailCoverImage" src={song.songCoverUrl}></img>

    
    <div className="songDetailsMain">
            <button className="playButton" onClick={
                function () {
                    const player = document.getElementById("songPlayer")
                    const audioPlayer = player.parentElement
                    player.src = `${song.url}`
                    audioPlayer.load()
                }}><img className="playButtonIcon" src="https://firebasestorage.googleapis.com/v0/b/hifi-ed258.appspot.com/o/images%2FPlayButton3.png?alt=media&token=16374b88-23e6-4c1a-843a-ed22878773f2" alt="playButtonIcon"></img>
        </button>


</div>
            <h3 className="song__name">{song.name}</h3>
           <Link to={`/users/${song.userId}`}>

                <div className="song__user">{user.name}</div>
            </Link>



 

            {renderDeleteButton()}

            {renderEditButton()}

        </section>
    )
}