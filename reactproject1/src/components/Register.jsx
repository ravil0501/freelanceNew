import { useState } from 'react';
import axios from 'axios';

const Register = () => {
    const [form, setForm] = useState({
        email: '',
        username: '',
        password: '',
        role: 'Freelancer',
    });

    const [error, setError] = useState('');
    const [success, setSuccess] = useState('');

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        setSuccess('');

        try {
            const res = await axios.post('/api/Auth/register', form, {
                headers: { 'Content-Type': 'application/json' }
            });
            setSuccess('Регистрация успешна!');
        } catch (err) {
            console.error('Ошибка регистрации:', err);

            if (err.response?.data) {
                if (typeof err.response.data === 'string') {
                    setError(err.response.data);
                } else {
                    setError(JSON.stringify(err.response.data, null, 2));
                }
            } else {
                setError(err.message || 'Неизвестная ошибка');
            }
        }
    };

    return (
        <div className="form-container">
            <h2>Регистрация</h2>
            <form onSubmit={handleSubmit}>
                <input type="email" name="email" placeholder="Email" value={form.email} onChange={handleChange} required />
                <input type="text" name="username" placeholder="Username" value={form.username} onChange={handleChange} required />
                <input type="password" name="password" placeholder="Password" value={form.password} onChange={handleChange} required />
                <select name="role" value={form.role} onChange={handleChange}>
                    <option value="Freelancer">Фрилансер</option>
                    <option value="Client">Клиент</option>
                </select>
                <button type="submit">Зарегистрироваться</button>
            </form>
            {error && <p className="error">{error}</p>}
            {success && <p className="success">{success}</p>}
        </div>
    );
};

export default Register;