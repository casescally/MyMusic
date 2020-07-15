import React, { useState, useEffect } from "react"

export const UserContext = React.createContext()

export const UserProvider = (props) => {
    const [users, setUsers] = useState([])

    const getUsers = () => {
        return fetch("https://localhost:5001/api/users")
            .then(res => res.json())
            .then(parsedUsers => {
                setUsers(parsedUsers)
            })
    }

    const addUser = user => {
        return fetch("https://localhost:5001/api/users", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(user)
        })
            .then(getUsers)
    }

    useEffect(() => {
        getUsers()
    }, [])

    useEffect(() => {
        //console.log("****  USER APPLICATION STATE CHANGED  ****")
        //console.log(users)
    }, [users])

    return (
        <UserContext.Provider value={{
            users, addUser
        }}>
            {props.children}
        </UserContext.Provider>
    )
}